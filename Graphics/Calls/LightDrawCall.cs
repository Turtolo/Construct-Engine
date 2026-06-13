using Amethyst.Params;
using Amethyst.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Graphics
{
  public sealed class PointLightDrawCall : Layered, IDrawCall, IPoolable 
  {
    public int Index { get; set; }

    public MTexture Texture { get; set; }
    public Rectangle? SourceRectangle { get; set; }

    public float Intensity { get; set; }
    public Color Tint { get; set; } = Color.White;

    public Effect Effect { get; set; }

    public CanvasParams Params { get; set; } = CanvasParams.Identity;

    public BatchKey Key { get; set; } = BatchKey.Default;

    public void Draw(SpriteBatch sb)
    {
      if (Texture?.Texture == null)
        return;

      Rectangle src =
          SourceRectangle
          ?? Texture.SourceRectangle
          ?? new Rectangle(0, 0, Texture.Texture.Width, Texture.Texture.Height);

      Color color = Tint * Intensity;

      //Perhaps, though i will have to research this further.
      //Color color = ColorExstention.Multiply(Tint, Params.Color) * Tint

      sb.Draw(
          Texture.Texture,
          Params.Position,
          src,
          color,
          Params.Rotation,
          Params.Origin,
          Params.Scale,
          Params.Effects,
          InternalDepth
      );
    }

    public void Reset()
    {
      Texture = null;
      SourceRectangle = null;
      Effect = null;
      Params = CanvasParams.Identity;
      Key = BatchKey.Default;
      Depth = 0;

      ObjectPool<PointLightDrawCall>.Return(this);
    }
  }
}
