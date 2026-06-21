using Amethyst.Managers;
#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Amethyst.Graphics;
using Amethyst.Managers;
using Amethyst.Params;
using Amethyst.Hierarchy;

namespace Amethyst.Geometry
{
  public class RayCastShape2D
  {
    public Vector2 TargetPosition { get; set; }

    public Vector2 Direction
    {
      get
      {
        return TargetPosition == Vector2.Zero
            ? Vector2.Zero
            : Vector2.Normalize(TargetPosition);
      }
    }

    public float Length { get => TargetPosition.Length(); }
    
    public RayCastShape2D(Vector2 targetPosition)
    {
      TargetPosition = targetPosition;
    }

    public bool CheckIntersections(IShape2D other, Vector2 thisPosition, Vector2 otherPosition, out Vector2 hitPoint, out float distance)
    {
      float closest = float.MaxValue;

      if (other.RayIntersect(
          thisPosition,
          Direction,
          Length,
          otherPosition,
          out hitPoint,
          out distance))
      {
        if (distance < closest)
        {
          return true;
        }
      }

      return false;
    }
  }
}
