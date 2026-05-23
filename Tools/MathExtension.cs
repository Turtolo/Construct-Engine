using System;
using Microsoft.Xna.Framework;

namespace Amethyst.Tools
{
  public static class MathE
  {
    public static Random Random { get; set; } = new();

    public static float RandomFloat(float min, float max)
    {
      return (float)(Random.NextDouble() * (max - min)) + min;
    }

    public static int Lerp(int start, int end, float t)
    {
      return (int)MathF.Round(start + (end - start) * t);
    }

    public static Point Lerp(Point start, Point end, float t)
    {
      return new Point(
          (int)MathF.Round(start.X + (end.X - start.X) * t),
          (int)MathF.Round(start.Y + (end.Y - start.Y) * t)
      );
    }

  }
}
