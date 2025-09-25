using System.Collections.Generic;
using ConstructEngine.Components.Object;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Physics
{
    public class ColliderObject
    {
        public static List<ColliderObject> ColliderList = new List<ColliderObject>();
        
        public Rectangle Rect;
        public bool IsSolid;
        public bool OneWay;
        public Vector2 Velocity = Vector2.Zero;
        protected Texture2D pixel;

        public ColliderObject(Rectangle rect, bool isSolid)
        {
            Rect = rect;
            IsSolid = isSolid;

            ColliderList.Add(this);
        }

        public void ChangeCollisionState(bool state)
        {
            IsSolid = state;
        }

        public virtual void Update() { }

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