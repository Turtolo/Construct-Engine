using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Graphics;
using Amethyst.Params;

namespace Amethyst.Hierarchy
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

    public override void _SubmitCall()
    {
      if (Texture == null || Material.Global.Visible == false)
        return;

      Rectangle sourceRect = SourceRect;

      Vector2 pos = Rounded ? Vector2.Floor(Transform.Global.Position) : Transform.Global.Position;
      Vector2 scale = Rounded ? Vector2.Floor(Transform.Global.Scale) : Transform.Global.Scale;
      
      Core.Canvas.Submit(new TextureDrawCall
      {
        Texture = Texture,
        SourceRectangle = sourceRect,

        Params = CanvasParams.Identity with
        {
          Position = pos,
          Color = Material.Global.Modulate,
          Rotation = Transform.Global.Rotation,
          Origin = new Vector2(
            sourceRect.Width / 2f,
            sourceRect.Height / 2f
          ),
          Scale = scale,
          Effects = Material.Global.SpriteEffects,
        },
        Key = BatchKey.Default with
        {
          Matrix = Seperated ? null : Core.Index.Get<Camera2D>().GetTransform()
        },
        Depth = Ordering.Global.Depth,
        Effect = Material.Global.Shader
      });
    }
  }
}
