using System.Collections.Generic;
using ConstructEngine.Object;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Physics
{
    public class Collider
    {
        public static List<Collider> ColliderList = new List<Collider>();

        public Rectangle Rect;
        public Circle Circ;

        public bool Enabled;
        public bool OneWay;
        
        public Vector2 Velocity = Vector2.Zero;
        protected Texture2D pixel;

        public Collider(Rectangle rect, bool enabled, bool oneWay = false)
        {
            Rect = rect;
            Enabled = enabled;
            OneWay = oneWay;

            ColliderList.Add(this);
        }
        
        public Collider(Circle circle, bool enabled, bool oneWay = false)
        {
            Circ = circle;
            Enabled = enabled;
            OneWay = oneWay;
            
            ColliderList.Add(this);
        }

        public void ChangeCollisionState(bool state)
        {
            Enabled = state;
        }

        public virtual void Update(GameTime gameTime)
        {
 
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