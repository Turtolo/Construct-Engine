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

    SpriteBatchParams BatchParams { get; }

    int Depth { get; }

    void Draw(SpriteBatch sb);
  }
}
