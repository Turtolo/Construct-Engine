using System;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Area
{
    public class Circle : IEquatable<Circle>
    {
        private static readonly Circle s_empty = new Circle(0, 0, 0);
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }
        private bool Enabled { get; set; }

        public Point Location
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public static Circle Empty => s_empty;
        public bool IsEmpty => X == 0 && Y == 0 && Radius == 0;

        public int Top => Y - Radius;
        public int Bottom => Y + Radius;
        public int Left => X - Radius;
        public int Right => X + Radius;

        public Circle(int x, int y, int radius, bool enabled = true)
        {
            Enabled = enabled;
            X = x;
            Y = y;
            Radius = radius;
        }

        public Circle(Point location, int radius, bool enabled = true)
        {
            Enabled = enabled;
            X = location.X;
            Y = location.Y;
            Radius = radius;
        }

        /// <summary>
        /// Checks for intersection with other circles
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>

        public bool Intersects(Circle other)
        {
            int radiiSquared = (Radius + other.Radius) * (Radius + other.Radius);
            float distanceSquared = Vector2.DistanceSquared(Location.ToVector2(), other.Location.ToVector2());
            return distanceSquared < radiiSquared;
        }

        public override bool Equals(object obj) => obj is Circle other && Equals(other);

        public bool Equals(Circle other)
        {
            if (other is null) return false;

            return X == other.X &&
                   Y == other.Y &&
                   Radius == other.Radius;
        }

        public override int GetHashCode() => HashCode.Combine(X, Y, Radius);

        public static bool operator ==(Circle lhs, Circle rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (lhs is null || rhs is null) return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Circle lhs, Circle rhs) => !(lhs == rhs);
    }
}
