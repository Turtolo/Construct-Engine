using System;
using Microsoft.Xna.Framework;
using ConstructEngine.Physics;

namespace ConstructEngine.Util;

public enum CollisionSide
{
    None,
    Left,
    Right,
    Top,
    Bottom
}

public static class CollisionHelper
{
    public static bool IsRectangleCollidingCircle(Circle circ, Rectangle rect)
    {
                Vector2 closestPoint = new Vector2(
            MathHelper.Clamp(circ.Location.X, rect.Left, rect.Right),
            MathHelper.Clamp(circ.Location.Y, rect.Top, rect.Bottom)
        );

        float distance = Vector2.Distance(circ.Location.ToVector2(), closestPoint);

        return distance <= circ.Radius;
    }
    public static CollisionSide GetCameraEdge(Rectangle target, Rectangle camera)
    {
        if (target.Right > camera.Right)
        {
            return CollisionSide.Right;
        }

        if (target.Left < camera.Left)
        {
            return CollisionSide.Left;
        }

        if (target.Top < camera.Top)
        {
            return CollisionSide.Top;
        }

        if (target.Bottom > camera.Bottom)
        {
            return CollisionSide.Bottom;
        }

        return CollisionSide.None;
    }


}