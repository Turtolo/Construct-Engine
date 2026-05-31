using System;
using System.Collections.Generic;
using Amethyst.Geometry;
using Microsoft.Xna.Framework;

namespace Amethyst.Tools
{
  public static class ShapeT
  {

    public static float DistanceSquared(Point a, Point b)
    {
      float dx = a.X - b.X;
      float dy = a.Y - b.Y;

      return dx * dx + dy * dy;
    }

    public static bool IsCircleIntersectingConvex(Vector2 circleCenter, float circleRadius, List<Vector2> vertices)
    {
      bool isInside = true;
      int count = vertices.Count;

      for (int i = 0; i < count; i++)
      {
        Vector2 pointA = vertices[i];
        Vector2 pointB = vertices[(i + 1) % count];

        Vector2 closestPoint = GetClosestPointOnSegment(pointA, pointB, circleCenter);

        float distanceSquared = Vector2.DistanceSquared(circleCenter, closestPoint);

        if (distanceSquared <= circleRadius * circleRadius)
          return true;

        Vector2 dist = pointB - pointA;
        Vector2 toCircle = circleCenter - pointA;

        float cross = (dist.X * toCircle.Y) - (dist.Y * toCircle.X);

        if (cross < 0)
          isInside = false;
      }

      return isInside;
    }

    public static Vector2 GetClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 p)
    {
      Vector2 ab = b - a;
      Vector2 ap = p - a;

      float t = Vector2.Dot(ab, ap) / Vector2.Dot(ab, ab);

      t = Math.Clamp(t, 0f, 1f);

      return a + t * ab;
    }
  }
}
