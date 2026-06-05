#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Params;
using Amethyst.Util;

namespace Amethyst.Graphics
{
  public sealed class FontDrawCall : Layered, IDrawCall, IPoolable
  {
    public int Index { get; set;}

    public IFont Font { get; set; }
    public string Text { get; set; }

    public Effect Effect { get; set; }

    public CanvasParams Params { get; set; } = CanvasParams.Identity;

    public BatchKey Key { get; set; } = BatchKey.Default;

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

    public void Reset()
    {
      Font = null;
      Text = null;
      Effect = null;
      Params = CanvasParams.Identity;
      Key = BatchKey.Default;
      Depth = 0;

      ObjectPool<FontDrawCall>.Return(this);
    }

  }
}
