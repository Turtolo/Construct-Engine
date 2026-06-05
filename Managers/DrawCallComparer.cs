using System.Collections.Generic;
using Amethyst.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Managers
{
  public class DrawCallComparer : IComparer<IDrawCall>
  {
      public static readonly DrawCallComparer Instance = new();

      public int Compare(IDrawCall? x, IDrawCall? y)
      {
          if (ReferenceEquals(x, y)) return 0;
          if (x == null) return -1;
          if (y == null) return 1;

          int depthCompare = x.Depth.CompareTo(y.Depth);
          if (depthCompare != 0) return depthCompare;

          int effectCompare = Comparer<Effect>.Default.Compare(x.Effect, y.Effect);
          if (effectCompare != 0) return effectCompare;

          return x.Key.GetHashCode().CompareTo(y.Key.GetHashCode());
      }
  }
}
