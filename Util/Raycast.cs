using Microsoft.Xna.Framework;
using System.Collections.Generic;

public static class Raycast
{
    /// <summary>
    /// Checks for the closest intersection between a ray and a list of rectangles.
    /// </summary>
    /// <param name="ray">The Ray2D to cast.</param>
    /// <param name="rectangles">The list of rectangles to check against.</param>
    /// <param name="hitPoint">The intersection point on the closest rectangle.</param>
    /// <returns>True if a collision was found, false otherwise.</returns>
    public static bool RaycastToRectangles(Ray2D ray, List<Rectangle> rectangles, out Vector2 hitPoint)
    {
        float closestDistance = float.MaxValue;
        bool hasHit = false;
        hitPoint = Vector2.Zero;

        foreach (var rect in rectangles)
        {
            if (CheckRaycast(ray, rect, out Vector2 currentHitPoint, out float currentDistance))
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

    /// <summary>
    /// Checks for a single intersection between a ray and one rectangle.
    /// </summary>
    /// <param name="ray">The Ray2D to cast.</param>
    /// <param name="rect">The rectangle to check against.</param>
    /// <param name="hitPoint">The intersection point if a collision occurred.</param>
    /// <param name="distance">The distance to the intersection point.</param>
    /// <returns>True if an intersection occurred, false otherwise.</returns>
    private static bool CheckRaycast(Ray2D ray, Rectangle rect, out Vector2 hitPoint, out float distance)
    {
        distance = float.MaxValue;
        hitPoint = Vector2.Zero;
        bool hasHit = false;

        Vector2[] corners = new Vector2[4];
        corners[0] = new Vector2(rect.Left, rect.Top);
        corners[1] = new Vector2(rect.Right, rect.Top);
        corners[2] = new Vector2(rect.Right, rect.Bottom);
        corners[3] = new Vector2(rect.Left, rect.Bottom);

        for (int i = 0; i < 4; i++)
        {
            Vector2 p1 = corners[i];
            Vector2 p2 = corners[(i + 1) % 4];

            if (LineIntersects(ray.Position, ray.Position + ray.Direction * 1000, p1, p2, out Vector2 intersectionPoint))
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

    /// <summary>
    /// Calculates the intersection of two line segments.
    /// Uses the standard line-line intersection formula.
    /// </summary>
    private static bool LineIntersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersectionPoint)
    {
        intersectionPoint = Vector2.Zero;
        float denominator = ((p4.Y - p3.Y) * (p2.X - p1.X)) - ((p4.X - p3.X) * (p2.Y - p1.Y));

        // If the denominator is zero, the lines are parallel.
        if (denominator == 0)
        {
            return false;
        }

        float u_a = (((p4.X - p3.X) * (p1.Y - p3.Y)) - ((p4.Y - p3.Y) * (p1.X - p3.X))) / denominator;
        float u_b = (((p2.X - p1.X) * (p1.Y - p3.Y)) - ((p2.Y - p1.Y) * (p1.X - p3.X))) / denominator;

        // If u_a is between 0 and 1, the intersection point is on our line segment.
        // The ray is a line, so u_b can be any value > 0 (as long as we limit the ray's length).
        if (u_a >= 0 && u_a <= 1 && u_b >= 0)
        {
            intersectionPoint.X = p1.X + (u_a * (p2.X - p1.X));
            intersectionPoint.Y = p1.Y + (u_a * (p2.Y - p1.Y));
            return true;
        }

        return false;
    }
}
