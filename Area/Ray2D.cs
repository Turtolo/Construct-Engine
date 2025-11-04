using System;
using System.Collections.Generic;
using ConstructEngine.Area;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Area
{
    public class Ray2D
    {
        public static List<Ray2D> RayList = new();
        public Vector2 Position { get; set; }
        public float AngleDegrees { get; set; }
        public float Length { get; set; }
        private bool _hasHit;
        private Vector2 _hitPoint;

        public bool HasHit => _hasHit;
        public Vector2 HitPoint => _hitPoint;

        public Vector2 Direction => new Vector2(
            MathF.Cos(MathHelper.ToRadians(AngleDegrees)),
            MathF.Sin(MathHelper.ToRadians(AngleDegrees))
        );

        public Ray2D(Vector2 position, float angleDegrees, float length)
        {
            Position = position;
            AngleDegrees = angleDegrees;
            Length = length;
            _hasHit = false;
            _hitPoint = Vector2.Zero;

            RayList.Add(this);
        }

        public void Update(Vector2 pos, float angleDegrees)
        {
            Position = pos;
            AngleDegrees = angleDegrees;
        }

        public bool Raycast(List<Area2D> areas)
        {
            float maxLength = Length;
            float closestDistance = float.MaxValue;
            _hasHit = false;
            _hitPoint = Vector2.Zero;

            foreach (var area in areas)
            {
                Vector2 currentHitPoint;
                float currentDistance;
                bool hit = false;

                hit = CheckRaycast(this, area, maxLength, out currentHitPoint, out currentDistance);

                if (hit && currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    _hitPoint = currentHitPoint;
                    _hasHit = true;
                }
            }

            return _hasHit;
        }
        

        public bool Raycast(List<Area2D> areas, Type target)
        {
            float maxLength = Length;
            float closestDistance = float.MaxValue;
            _hasHit = false;
            _hitPoint = Vector2.Zero;

            foreach (var area in areas)
            {
                if (area.RootType != target) continue;

                Vector2 currentHitPoint;
                float currentDistance;
                bool hit = false;

                hit = CheckRaycast(this, area, maxLength, out currentHitPoint, out currentDistance);
            
                if (hit && currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    _hitPoint = currentHitPoint;
                    _hasHit = true;
                }
            }

            return _hasHit;
        }


        private static bool RectangleIntersect(Ray2D ray, Rectangle rect, float maxLength, out Vector2 hitPoint, out float distance)
        {
            hitPoint = Vector2.Zero;
            distance = float.MaxValue;
            bool hasHit = false;

            Vector2 rayEnd = ray.Position + ray.Direction * maxLength;

            Vector2[] corners =
            {
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Right, rect.Top),
                new Vector2(rect.Right, rect.Bottom),
                new Vector2(rect.Left, rect.Bottom)
            };

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

        private static bool CircleIntersect(Ray2D ray, Circle circ, float maxLength, out Vector2 hitPoint, out float distance)
        {
            hitPoint = Vector2.Zero;
            distance = float.MaxValue;

            float t = Vector2.Dot(circ.Location.ToVector2() - ray.Position, ray.Direction);

            Vector2 closestPoint = ray.Position + ray.Direction * t;
            float distToCircle = Vector2.Distance(closestPoint, circ.Location.ToVector2());

            if (distToCircle > circ.Radius) return false;

            float dt = (float)Math.Sqrt(circ.Radius * circ.Radius - distToCircle * distToCircle);
            float t0 = t - dt;
            float t1 = t + dt;

            float hitT = (t0 >= 0) ? t0 : t1;
            if (hitT < 0) return false;

            hitPoint = ray.Position + ray.Direction * hitT;
            distance = hitT;

            return true;
        }


        private static bool CheckRaycast(Ray2D ray, Area2D area, float maxLength, out Vector2 hitPoint, out float distance)
        {
            if (area.Rect != null)
                return RectangleIntersect(ray, area.Rect, maxLength, out hitPoint, out distance);
            else if (area.Circ != null)
                return CircleIntersect(ray, area.Circ, maxLength, out hitPoint, out distance);

            hitPoint = Vector2.Zero;
            distance = float.MaxValue;
            return false;
        }

        private static bool LineIntersects(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersectionPoint)
        {
            intersectionPoint = Vector2.Zero;
            float denominator = ((p4.Y - p3.Y) * (p2.X - p1.X)) - ((p4.X - p3.X) * (p2.Y - p1.Y));

            if (Math.Abs(denominator) < float.Epsilon)
                return false;

            float uA = (((p4.X - p3.X) * (p1.Y - p3.Y)) - ((p4.Y - p3.Y) * (p1.X - p3.X))) / denominator;
            float uB = (((p2.X - p1.X) * (p1.Y - p3.Y)) - ((p2.Y - p1.Y) * (p1.X - p3.X))) / denominator;

            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                intersectionPoint = p1 + uA * (p2 - p1);
                return true;
            }

            return false;
        }
    }
}
