using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConstructEngine;
using ConstructEngine.Physics;

namespace Graphics;

public class RectangleDraw
{
    public RectangleDraw()
    {

    }

    private Texture2D rectangleTexture;
    private Texture2D rectangleColliderTexture;


    public void LoadContent()
    {
        rectangleTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new Color[] { Color.Red });

        rectangleColliderTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
        rectangleColliderTexture.SetData(new Color[] { Color.Red });

    }
    
    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color, float layerDepth)
    {
        Texture2D rectangleTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
        rectangleTexture.SetData(new[] { color });

        // Top line
        spriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.X, rect.Y, rect.Width, thickness),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        // Bottom line
        spriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        // Left line
        spriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.X, rect.Y, thickness, rect.Height),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);

        // Right line
        spriteBatch.Draw(rectangleTexture,
            new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height),
            null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
    }

    
    
}