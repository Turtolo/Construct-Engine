using Amethyst.Managers;
#nullable disable

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Amethyst.Geometry
{
  public static class Intersection
  {
    public static bool RectangleIntersectWithCircle(Point rectPos, RectangleShape2D rect, Point circlePos, CircleShape2D circle)
    {
      float closestX = Math.Clamp(circlePos.X, rectPos.X, rectPos.X + rect.Width);
      float closestY = Math.Clamp(circlePos.Y, rectPos.Y, rectPos.Y + rect.Height);

      float distanceX = circlePos.X - closestX;
      float distanceY = circlePos.Y - closestY;
      float distanceSquared = distanceX * distanceX + distanceY * distanceY;

      return distanceSquared <= circle.Radius * circle.Radius;
    }


    public static bool RayIntersectWithRectangle(
        RectangleShape2D rectangle,
        Vector2 rayOrigin,
        Vector2 rayDir,
        float maxLength,
        Vector2 position,
        out Vector2 hitPoint,
        out float distance)
    {
      hitPoint = Vector2.Zero;
      distance = 0f;

      Rectangle r = rectangle.GetAABB(position.ToPoint());

      float tmin = 0f;
      float tmax = maxLength;

      if (rayDir.X != 0f)
      {
        float inv = 1f / rayDir.X;
        float t1 = (r.Left - rayOrigin.X) * inv;
        float t2 = (r.Right - rayOrigin.X) * inv;

        if (t1 > t2) (t1, t2) = (t2, t1);

        tmin = MathF.Max(tmin, t1);
        tmax = MathF.Min(tmax, t2);

        if (tmin > tmax) return false;
      }
      else if (rayOrigin.X < r.Left || rayOrigin.X > r.Right)
      {
        return false;
      }

      if (rayDir.Y != 0f)
      {
        float inv = 1f / rayDir.Y;
        float t1 = (r.Top - rayOrigin.Y) * inv;
        float t2 = (r.Bottom - rayOrigin.Y) * inv;

        if (t1 > t2) (t1, t2) = (t2, t1);

        tmin = MathF.Max(tmin, t1);
        tmax = MathF.Min(tmax, t2);

        if (tmin > tmax) return false;
      }
      else if (rayOrigin.Y < r.Top || rayOrigin.Y > r.Bottom)
      {
        return false;
      }

      distance = tmin;
      hitPoint = rayOrigin + rayDir * tmin;

      return true;
    }
  }
}
