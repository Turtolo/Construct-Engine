using System.Collections.Generic;
using System.Linq;
using ConstructEngine.Object;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConstructEngine.Util;
using System.Security.Cryptography.X509Certificates;
using System;

namespace ConstructEngine.Physics
{
    public class Collider
    {
        public static List<Collider> ColliderList = new List<Collider>();


        public Rectangle Rect;
        public static List<Rectangle> RectangleList = new List<Rectangle>();
        public Circle Circ;
        public static List<Circle> CircleList = new List<Circle>();

        public bool Enabled;
        public bool OneWay;
        
        public Vector2 Velocity = Vector2.Zero;
        protected Texture2D pixel;

        public Collider(Rectangle rect, bool enabled, bool oneWay = false)
        {
            Rect = rect;
            Enabled = enabled;
            OneWay = oneWay;

            RectangleList.Add(Rect);

            ColliderList.Add(this);
        }
        
        public Collider(Circle circle, bool enabled, bool oneWay = false)
        {
            Circ = circle;
            Enabled = enabled;
            OneWay = oneWay;
            
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