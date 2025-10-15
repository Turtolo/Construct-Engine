using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Graphics;

public class ColliderDraw()
{
    public static void DrawCircle(Circle circ, Color color, int thickness, float layerDepth = 0.1f)
    {
        Texture2D texture = new Texture2D(Core.GraphicsDevice, circ.Radius, circ.Radius);
        Color[] colorData = new Color[circ.Radius * circ.Radius];

        float diam = circ.Radius / 2f;
        float diamsq = diam * diam;

        for (int x = 0; x < circ.Radius; x++)
        {
            for (int y = 0; y < circ.Radius; y++)
            {
                int index = x * circ.Radius + y;
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

        Core.SpriteBatch.Draw(texture, new Vector2(circ.X, circ.Y), color);
    }
    public static void DrawRectangle(Rectangle rect, Color color, int thickness, float layerDepth = 0.1f)
    {
        Texture2D rectangleTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { color });

        Core.SpriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.X, rect.Y, rect.Width, thickness),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        Core.SpriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        Core.SpriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.X, rect.Y, thickness, rect.Height),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        Core.SpriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }
}