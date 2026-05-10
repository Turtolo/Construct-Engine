using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Params;

namespace Amethyst.Graphics
{
  public sealed class FontDrawCall : Layered, IDrawCall
  {
    public required IFont Font { get; init; }
    public string Text { get; init; }

    public CanvasParams Params { get; set; } = CanvasParams.Identity;

    public BatchKey Key { get; init; } = BatchKey.Default;

    public void Draw(SpriteBatch sb)
    {
      if (Font == null || string.IsNullOrEmpty(Text))
        return;

      Font.DrawString(
          sb,
          Text,
          Params.Position,
          Params.Color,
          Params.Rotation,
          Params.Origin,
          Params.Scale,
          Params.Effects,
          InternalDepth
      );
    }

  }
}
