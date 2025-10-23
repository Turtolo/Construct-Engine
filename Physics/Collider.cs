using System.Collections.Generic;
using System.Linq;
using ConstructEngine.Object;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConstructEngine.Util;
using System.Security.Cryptography.X509Certificates;
using System;
using ConstructEngine.Components.Entity;

namespace ConstructEngine.Physics
{
    public class Collider
    {
        public static List<Collider> ColliderList = new List<Collider>();
        public static List<Circle> CircleList = new List<Circle>();
        public static List<Rectangle> RectangleList = new List<Rectangle>();
        public static List<Collider> CollisionList = new List<Collider>();

        public Rectangle Rect;
        public Circle Circ;

        public object Root;

        public bool Enabled;
        
        public Vector2 Velocity = Vector2.Zero;
        protected Texture2D pixel;

        public Collider(Rectangle rect, bool enabled, object root)
        {
            Rect = rect;
            Enabled = enabled;

            Root = root;

            RectangleList.Add(Rect);

            ColliderList.Add(this);
        }
        
        public Collider(Circle circle, bool enabled, object root)
        {
            Circ = circle;
            Enabled = enabled;

            Root = root;
            
            CircleList.Add(Circ);

            ColliderList.Add(this);
        }

        public void ChangeCollisionState(bool state)
        {
            Enabled = state;
        }

        public virtual void Update(GameTime gameTime)
        {
            for (int i = 10; i <= 100; i++)
            {

            }

        }

        public void Free()
        {
            ColliderList.Remove(this);
        }
        
        public bool HasRect
        {
            get { return Rect != default; }
        }

        public bool HasCircle
        {
            get { return Circ != null; }
        }

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



        

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(device, 1, 1);
                pixel.SetData(new[] { Color.White });
            }
            spriteBatch.Draw(pixel, Rect, Color.Gray);
        }
    }
}