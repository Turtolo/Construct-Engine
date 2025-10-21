using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Util;

public struct Ray2D
{
    public Vector2 Position;
    public Vector2 Direction;
    public int Length;

    static bool hasHit = false;


    public Ray2D(Vector2 position, Vector2 direction, int length)
    {
        Position = position;
        Direction = Vector2.Normalize(direction);
        Length = length;
    }

    public bool IsColliding()
    {
        return hasHit;
    }

    public static bool RaycastToRectangles(Ray2D ray, List<Rectangle> rectangles, out Vector2 hitPoint)
    {
            float maxLength = ray.Length;
            float closestDistance = float.MaxValue;
            hasHit = false;
            hitPoint = Vector2.Zero;

            foreach (var rect in rectangles)
            {
                if (CheckRaycast(ray, rect, maxLength, out Vector2 currentHitPoint, out float currentDistance))
                {
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        hitPoint = currentHitPoint;
                        hasHit = true;
                    }
                }
            }
            return hasHit;
        }

        private static bool CheckRaycast(Ray2D ray, Rectangle rect, float maxLength, out Vector2 hitPoint, out float distance)
        {
            distance = float.MaxValue;
            hitPoint = Vector2.Zero;
            bool hasHit = false;

            Vector2 rayEnd = ray.Position + ray.Direction * maxLength;

            Vector2[] corners = new Vector2[4];
            corners[0] = new Vector2(rect.Left, rect.Top);
            corners[1] = new Vector2(rect.Right, rect.Top);
            corners[2] = new Vector2(rect.Right, rect.Bottom);
            corners[3] = new Vector2(rect.Left, rect.Bottom);

            for (int i = 0; i < 4; i++)
            {
                Vector2 p1 = corners[i];
                Vector2 p2 = corners[(i + 1) % 4];

                if (LineIntersects(ray.Position, rayEnd, p1, p2, out Vector2 intersectionPoint))
                {
                    float currentDistance = Vector2.Distance(ray.Position, intersectionPoint);
                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        hitPoint = intersectionPoint;
                        hasHit = true;
                    }
                }
            }
            return hasHit;
        }

        private static bool LineIntersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.Zero;
            float denominator = ((p4.Y - p3.Y) * (p2.X - p1.X)) - ((p4.X - p3.X) * (p2.Y - p1.Y));

            if (denominator == 0)
            {
                return false;
            }

            float u_a = (((p4.X - p3.X) * (p1.Y - p3.Y)) - ((p4.Y - p3.Y) * (p1.X - p3.X))) / denominator;
            float u_b = (((p2.X - p1.X) * (p1.Y - p3.Y)) - ((p2.Y - p1.Y) * (p1.X - p3.X))) / denominator;

            if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
            {
                intersectionPoint.X = p1.X + (u_a * (p2.X - p1.X));
                intersectionPoint.Y = p1.Y + (u_a * (p2.Y - p1.Y));
                return true;
            }

            return false;
        }
}
