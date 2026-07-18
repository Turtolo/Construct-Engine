using Opal.Managers;
#nullable disable

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Graphics;
using Opal.Params;

namespace Opal.Graphics
{
  public interface IDrawCall
  {
    CanvasParams Params { get; }

    public Effect Effect { get; }

    public int Index { get; set; }

    BatchKey Key { get; }

    int Depth { get; }

    void Draw(SpriteBatch sb);

    void Reset();
  }
}
