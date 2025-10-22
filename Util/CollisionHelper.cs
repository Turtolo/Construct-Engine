using System;
using Microsoft.Xna.Framework;
using ConstructEngine.Physics;
using System.Collections.Generic;
using System.Linq;

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

    public static bool IsRectangleEmpty(Rectangle collider)
    {
        if (collider.X == 0 && collider.Y == 0 && collider.Width == 0 && collider.Height == 0)
        {
            return true;
        }

        return false;

    }

    public static bool IsIntersectingAny(Collider collider)
    {


        bool intersects = Collider.ColliderList.Any(r => r.Rect.Intersects(collider.Rect));


        foreach (var otherCollider in Collider.ColliderList)
        {
            
            if (collider.Circ != null && otherCollider.Circ != null)
            {
                return collider.Circ.Intersects(otherCollider.Circ);
            }

            if (IsRectangleEmpty(collider.Rect) && !IsRectangleEmpty(otherCollider.Rect))
            {
                return collider.Rect.Intersects(otherCollider.Rect);
            }

            if (!IsRectangleEmpty(otherCollider.Rect) && collider.Circ != null)
            {
                return CircleIntersectsRectangle(collider.Circ, otherCollider.Rect);
            }
        }
        return false;
    }
        
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