using Opal.Managers;
#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Params;
using Opal.Util;
using System;

namespace Opal.Graphics
{
  public sealed class TextureDrawCall : Layered, IDrawCall, IPoolable
  {
    public int Index { get; set; }

    public MTexture Texture { get; set; }
    public Rectangle? SourceRectangle { get; set; }

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
      
      sb.Draw(
          Texture.Texture,
          Params.Position,
          src,
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
      Texture = null;
      SourceRectangle = null;
      Effect = null;
      Params = CanvasParams.Identity;
      Key = BatchKey.Default;
      Depth = 0;

      ObjectPool<TextureDrawCall>.Return(this);
    }
  }
}
