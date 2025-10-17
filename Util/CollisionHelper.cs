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
    public static bool CircleIntersectsRectangle(Circle circle, Rectangle rect)
    {
        int closestX = Math.Clamp(circle.X, rect.Left, rect.Right);
        int closestY = Math.Clamp(circle.Y, rect.Top, rect.Bottom);

        int deltaX = circle.X - closestX;
        int deltaY = circle.Y - closestY;

        return (deltaX * deltaX + deltaY * deltaY) <= (circle.Radius * circle.Radius);
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