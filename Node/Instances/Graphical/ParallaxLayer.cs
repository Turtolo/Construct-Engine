#nullable disable

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Graphics;
using Amethyst.Managers;
using Amethyst.Hierarchy;
using Amethyst.Params;
using Amethyst.Util;

namespace Amethyst.Hierarchy
{
  public enum LoopAxis
  {
    None = 0,
    X = 1 << 0,
    Y = 1 << 1,
    Both = X | Y
  }

  /// <summary>
  /// Represents a single infinite scrolling parallax layer.
  /// </summary>
  public class ParallaxLayer : Node2D
  {
    [Export]
    public MTexture Texture { get; set; }
    [Export]
    public Vector2 MotionScale { get; set; } = Vector2.One;
    [Export]
    public LoopAxis LoopAxes { get; set; } = LoopAxis.Both;

    private Vector2 lastCameraPos;
    private Vector2 offset;

    public ParallaxLayer() { }

    public void ApplyCameraDelta(Vector2 cameraDelta)
    {
      offset += cameraDelta * MotionScale;

      if (LoopAxes.HasFlag(LoopAxis.X))
        offset.X = Mod(offset.X, Texture.Bounds.Width);

      if (LoopAxes.HasFlag(LoopAxis.Y))
        offset.Y = Mod(offset.Y, Texture.Bounds.Height);

      if (!LoopAxes.HasFlag(LoopAxis.X))
        offset.X = 0;
      if (!LoopAxes.HasFlag(LoopAxis.Y))
        offset.Y = 0;
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      var camera = Core.Token.Get<Camera2D>();
      Vector2 camDelta = camera.Transform.Global.Position - lastCameraPos;
      lastCameraPos = camera.Transform.Global.Position;

      ApplyCameraDelta(camDelta);
    }

    public override void _SubmitCall()
    {
      if (!Material.Global.Visible)
        return;

      Rectangle view = Core.Token.Get<Camera2D>().GetWorldViewRectangle(false);

      int texW = Texture.Bounds.Width;
      int texH = Texture.Bounds.Height;

      Vector2 basePos = new Vector2(
          LoopAxes.HasFlag(LoopAxis.X)
              ? Transform.Global.Position.X - Mod(Transform.Global.Position.X - offset.X, Texture.Bounds.Width)
              : Transform.Global.Position.X,
          LoopAxes.HasFlag(LoopAxis.Y)
              ? Transform.Global.Position.Y - Mod(Transform.Global.Position.Y - offset.Y, Texture.Bounds.Height)
              : Transform.Global.Position.Y
      );

      int startX = LoopAxes.HasFlag(LoopAxis.X)
          ? (int)Math.Floor((double)view.Left / texW) - 1
          : 0;

      int startY = LoopAxes.HasFlag(LoopAxis.Y)
          ? (int)Math.Floor((double)view.Top / texH) - 1
          : 0;

      int endX = LoopAxes.HasFlag(LoopAxis.X)
          ? (int)Math.Ceiling((double)view.Right / texW) + 1
          : 1;

      int endY = LoopAxes.HasFlag(LoopAxis.Y)
          ? (int)Math.Ceiling((double)view.Bottom / texH) + 1
          : 1;

      for (int y = startY; y < endY; y++)
      {
        for (int x = startX; x < endX; x++)
        {
          Vector2 pos = new(
              x * texW + basePos.X,
              y * texH + basePos.Y
          );

          Vector2 modPos = Rounded ? Vector2.Floor(pos) : pos;
          Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;

          var call = DrawCallPool<TextureDrawCall>.Get();

          call.Texture = this.Texture;
          call.Depth = Ordering.Global.Depth;
          call.Effect = Material.Global.Shader;

          call.Params = CanvasParams.Identity with
          {
            Position = modPos,
            Color = Material.Global.Modulate,
            Rotation = Transform.Global.Rotation,
            Scale = scale,
            Effects = Material.Global.SpriteEffects,
          };

          call.Key = BatchKey.Default with
          {
            Matrix = Seperated ? null : Core.Token.Get<Camera2D>().GetTransform()
          };

          Core.Canvas.Submit(call);
        }
      }
    }

    private static float Mod(float x, float m)
    {
      return (x % m + m) % m;
    }
  }
}
