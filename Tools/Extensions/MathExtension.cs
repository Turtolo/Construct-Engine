using Opal.Managers;
using System;
using Microsoft.Xna.Framework;

namespace Opal.Tools
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


    public static Point Lerp(Point value1, Point value2, float amount)
    {
      amount = MathHelper.Clamp(amount, 0f, 1f);

      int x = (int)MathF.Round(value1.X + (value2.X - value1.X) * amount);
      int y = (int)MathF.Round(value1.Y + (value2.Y - value1.Y) * amount);

      return new Point(x, y);
    }

    public static Point LerpPure(Point start, Point end, float t)
    {
      return new Point(
          (int)MathF.Round(start.X + (end.X - start.X) * t),
          (int)MathF.Round(start.Y + (end.Y - start.Y) * t)
      );
    }

  }
}
