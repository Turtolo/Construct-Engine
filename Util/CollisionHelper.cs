using System;
using Microsoft.Xna.Framework;

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