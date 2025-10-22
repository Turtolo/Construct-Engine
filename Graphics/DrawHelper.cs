using System;
using ConstructEngine.Physics;
using ConstructEngine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Graphics;

public class DrawHelper()
{
    public static void DrawCircle(Circle circ, Color color, int thickness, float layerDepth = 0.1f)
    {
        if (circ == null)
        {
            return;
        }

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
    public static void DrawRay(Ray2D ray, Color color, float thickness, float layerDepth = 0.1f)
    {
        

        Color Color;

        if (ray.IsColliding())
        {
            Color = Color.Red;
        }

        else
        {
            Color = color;
        }
        
        Texture2D pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color });

        Vector2 end = ray.Position + ray.Direction * ray.Length;
        Vector2 edge = end - ray.Position;
        float angle = (float)Math.Atan2(edge.Y, edge.X);



        Core.SpriteBatch.Draw(
            pixel,
            new Rectangle((int)ray.Position.X, (int)ray.Position.Y, (int)edge.Length(), (int)thickness),
            null,
            Color,
            angle,
            Vector2.Zero,
            SpriteEffects.None,
            layerDepth);
    }
    
    public static void DrawRectangle(Rectangle rect, Color color, int thickness, float layerDepth = 0.1f)
    {
        if (rect == null)
        {
            return;
        }

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