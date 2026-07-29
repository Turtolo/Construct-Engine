using Opal.Managers;
#nullable disable

using Opal.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Graphics;
using Opal.Tools;
using Opal.Params;

namespace Opal.Hierarchy
{
  public class Sprite2D : Node2D
  {
    [Export]
    public MTexture Texture { get; set; }

    [Export]
    public int HFrames { get; set; } = 1;

    [Export]
    public int VFrames { get; set; } = 1;

    [Export]
    public int Frame { get; set; } = 0;

    public int FrameWidth
    {
      get
      {
        if (Texture == null)
          return 0;

        return Texture.Bounds.Width / HFrames;
      }
    }

    public int FrameHeight
    {
      get
      {
        if (Texture == null)
          return 0;

        return Texture.Bounds.Height / VFrames;
      }
    }

    public Rectangle SourceRect
    {
      get
      {
        if (Texture == null)
          return Rectangle.Empty;

        int frameWidth = FrameWidth;
        int frameHeight = FrameHeight;

        int x = Frame % HFrames;
        int y = Frame / HFrames;

        return new Rectangle(
          x * frameWidth,
          y * frameHeight,
          frameWidth,
          frameHeight
        );
      }
    }

    public Sprite2D() { }

    public override void _Submit(Canvas2D canvas)
    {
      if (Texture == null || Material.Global.Visible == false)
        return;

      Color finalColor = ColorExtension.Multiply(Material.Global.SelfModulate, Material.Global.Modulate);

      Rectangle sourceRect = SourceRect;

      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;

      TextureDrawCall call = ObjectPool<TextureDrawCall>.Get();

      call.Texture = Texture;
      call.SourceRectangle = sourceRect;
      call.Effect = Material.Global.Shader;
      call.Depth = Ordering.Global.Depth;

      call.Params = CanvasParams.Identity with
      {
        Position = pos,
        Color = finalColor,
        Rotation = Transform.Global.Rotation,
        Origin = new Vector2(sourceRect.Width / 2f, sourceRect.Height / 2f),
        Scale = scale,
        Effects = Material.Global.SpriteEffects,
      };

      call.Key = BatchKey.Default with
      {
        Matrix = Seperated ? null : Core.Token.Get<Camera2D>()?.GetTransform(),
      };

      Core.Canvas.Submit(call);
    }
  }
}
