using System;
using Amethyst.Geometry;
using Amethyst.Graphics;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Tools;
using Amethyst.Util;
using Microsoft.Xna.Framework;

namespace Amethyst.Hierarchy
{
  public class Parallax2D : Node2D
  {
    public MTexture Texture { get; set; }

    public int RepeatTimes { get; set; } = 1;

    public Vector2 MotionScale { get; set; }

    public Extent RepeatSize { get; set; }

    private Vector2 cameraPos;
    private Rectangle cameraRect;

    public override void _Process(float delta)
    {
      base._Process(delta);
      
      var cam = Core.Token.Get<Camera2D>();

      cameraRect = cam.GetWorldViewRectangle();
      cameraPos = new Vector2(cam.Transform.Global.Position.X, cam.Transform.Global.Position.Y);
    }

    public override void _Submit(Canvas2D canvas)
    {
      base._Submit(canvas);

      Color finalColor = ColorExtension.Multiply(Material.Global.SelfModulate, Material.Global.Modulate);
      
      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;

      Vector2 viewportSize = new Vector2(cameraRect.Width, cameraRect.Height);      
      Vector2 layerOffset = cameraPos * MotionScale;
      
      float startX, endX, stepX;
      if (RepeatSize.Width <= 0)
      {
        startX = pos.X - layerOffset.X;
        endX = startX + 1;
        stepX = 1f;
      }
      else
      {
        startX = -layerOffset.X % RepeatSize.Width;
        if (startX > 0)
          startX -= RepeatSize.Width;
        startX += pos.X;
        endX = pos.X + (viewportSize.X * RepeatTimes);
        stepX = RepeatSize.Width;
      }

      float startY, endY, stepY;
      if (RepeatSize.Height <= 0)
      {
        startY = pos.Y - layerOffset.Y;
        endY = startY + 1;
        stepY = 1f;
      }
      else
      {
        startY = -layerOffset.Y % RepeatSize.Height;
        if (startY > 0)
          startY -= RepeatSize.Height;
        startY += pos.Y;
        endY = pos.Y + (viewportSize.Y * RepeatTimes);
        stepY = RepeatSize.Height;
      }

      for (float x = startX; x < endX; x += stepX)
      {
        for (float y = startY; y < endY; y += stepY)
        {
          TextureDrawCall call = ObjectPool<TextureDrawCall>.Get();
          
          call.Texture = Texture;
          call.Effect = Material.Global.Shader;
          call.Depth = Ordering.Global.Depth;

          call.Params = CanvasParams.Identity with
          {
            Position = new Vector2(x, y),
            Color = finalColor,
            Rotation = Transform.Global.Rotation,
            Origin = new Vector2(Texture.Bounds.Width / 2f, Texture.Bounds.Height / 2f),
            Scale = scale,
            Effects = Material.Global.SpriteEffects,
          };

          call.Key = BatchKey.Default with
          {
            Matrix = Seperated ? null : Core.Token.Get<Camera2D>()?.GetTransform(),
          };

          canvas.Submit(call);
        }
      }
    }
  }
}
