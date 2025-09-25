using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ConstructEngine.Physics;

namespace ConstructEngine.Graphics
{


    public class CircleDraw
    {

        private Texture2D circleTexture;


        private Texture2D createCircleText(int radius, GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        public void DrawCircle(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Circle circle, Color color)
        {
            circleTexture = createCircleText(circle.Radius, graphicsDevice);

            spriteBatch.Draw(circleTexture, new Vector2(circle.X, circle.Y), color);
        }
    }
}