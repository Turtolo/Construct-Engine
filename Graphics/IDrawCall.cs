#nullable disable

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Graphics;
using Amethyst.Params;

namespace Amethyst.Graphics
{
  public interface IDrawCall
  {
    CanvasParams Params { get; }

    public Effect Effect { get; }

    BatchKey Key { get; }

    int Depth { get; }

    void Draw(SpriteBatch sb);

    void Recycle();
  }
}
