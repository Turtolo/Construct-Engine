using System.Collections.Generic;
using System.Linq;
using ConstructEngine.Object;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConstructEngine.Util;
using System.Security.Cryptography.X509Certificates;
using System;
using ConstructEngine.Components.Entity;
using System.Xml.Serialization;

namespace ConstructEngine.Physics
{
    public class Collider
    {
        public static List<Collider> ColliderList = new List<Collider>();
        public static List<Circle> CircleList = new List<Circle>();
        public static List<Rectangle> RectangleList = new List<Rectangle>();
        public bool HasRect
        {
            get { return Rect != default; }
        }
        public bool HasCircle
        {
            get { return Circ != null; }
        }
        public Vector2 Position
        {
            get
            {
                if (HasCircle)
                    return new Vector2(Circ.X, Circ.Y);
                if (HasRect)
                    return new Vector2(Rect.X, Rect.Y);
                return Vector2.Zero;
            }
            set
            {
                if (HasCircle)
                {
                    Circ.X = (int)value.X;
                    Circ.Y = (int)value.Y;
                }
                else if (HasRect)
                {
                    var rect = Rect;
                    rect.X = (int)value.X;
                    rect.Y = (int)value.Y;
                    Rect = rect;
                }
            }
        }


        public Rectangle Rect;
        public Circle Circ;
        public object Root;

        public bool Enabled;

        public Vector2 Velocity = Vector2.Zero;

        /// <summary>
        /// A collider with a rect, includes paramteres for a root and whether or not it is enabled.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="enabled"></param>
        /// <param name="root"></param>
        public Collider(Rectangle rect, bool enabled, object root)
        {
            Rect = rect;
            Enabled = enabled;

            Root = root;

            RectangleList.Add(Rect);

            ColliderList.Add(this);
        }

        /// <summary>
        /// A collider with a circle, includes paramteres for a root and whether or not it is enabled.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="enabled"></param>
        /// <param name="root"></param>

        public Collider(Circle circle, bool enabled, object root)
        {
            Circ = circle;
            Enabled = enabled;

            Root = root;

            CircleList.Add(Circ);

            ColliderList.Add(this);
        }
        
        /// <summary>
        /// Frees the current collider immediately
        /// </summary>

        public void Free()
        {
            ColliderList.Remove(this);
        }

        /// <summary>
        /// Gets the information for the currently intersecting collider
        /// </summary>
        /// <returns></returns>
        
        public Collider GetCurrentlyIntersectingCollider()
        {
            if (!this.Enabled) return null;

            foreach (var other in ColliderList)
            {
                if (other == this || !other.Enabled) continue;

                if (this.HasRect && other.HasRect && this.Rect.Intersects(other.Rect))
                    return other;

                if (this.HasCircle && other.HasCircle && this.Circ.Intersects(other.Circ))
                    return other;

                if (this.HasRect && other.HasCircle && CollisionHelper.CircleIntersectsRectangle(other.Circ, this.Rect))
                    return other;

                if (this.HasCircle && other.HasRect && CollisionHelper.CircleIntersectsRectangle(this.Circ, other.Rect))
                    return other;
            }

            return null;
        }

        /// <summary>
        /// Checks if the collider is intersecting with any collider
        /// </summary>
        /// <returns></returns>

        public bool IsIntersectingAny()
        {
            if (!this.Enabled) return false;

            foreach (var other in ColliderList)
            {
                if (other == this || !other.Enabled) continue;

                if (this.HasRect && other.HasRect && this.Rect.Intersects(other.Rect)) return true;
                if (this.HasCircle && other.HasCircle && this.Circ.Intersects(other.Circ)) return true;
                if (this.HasRect && other.HasCircle && CollisionHelper.CircleIntersectsRectangle(other.Circ, this.Rect)) return true;
                if (this.HasCircle && other.HasRect && CollisionHelper.CircleIntersectsRectangle(this.Circ, other.Rect)) return true;
            }

            return false;
        }
    }
}