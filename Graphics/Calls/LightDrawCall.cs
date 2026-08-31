using Opal.Params;
using Opal.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Opal.Graphics
{
  public sealed class PointLightDrawCall : Layered, IDrawCall, IPoolable
  {
    public int Index { get; set; }

    public TextureRegion Texture { get; set; }
    public Rectangle? SourceRectangle { get; set; }

    public float Intensity { get; set; }
    public Color Tint { get; set; } = Color.White;

    public Effect Effect { get; set; }

    public CanvasParams Params { get; set; } = CanvasParams.Identity;

    public BatchKey Key { get; set; } = BatchKey.Default;

    public void Draw(SpriteBatch sb)
    {
      if (Texture.Source == null)
        return;

      Color color = Tint * Intensity;

      Rectangle src = new Rectangle
      (
        SourceRectangle?.Location + Texture.SourceRectangle.Location ?? Texture.SourceRectangle.Location, 
        SourceRectangle?.Size ?? Texture.SourceRectangle.Size
      );

      //Perhaps, though i will have to research this further.
      //Color color = ColorExstention.Multiply(Tint, Params.Color) * Tint

      sb.Draw(
          Texture.Source,
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
