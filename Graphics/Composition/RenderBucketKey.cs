#nullable disable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Graphics
{
  public readonly record struct RenderBucketKey
  {
    public readonly int Depth;
    public readonly BatchKey BatchKey;
    public readonly Effect Effect;

    public RenderBucketKey(int depth, BatchKey batchKey, Effect effect)
    {
      Depth = depth;
      BatchKey = batchKey;
      Effect = effect;
    }
  }
}
