using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Geometry;

namespace Opal.Graphics
{
  public class TextureRegion
  {
    /// <summary>
    /// The source atlas, containing multiple packed sprites; this represents a sub-section within it.
    /// </summary>
    public Texture2D Source { get; set; }
    
    /// <summary>
    /// The location, width and height of the sub-section within <see cref="Source"/>.
    /// </summary>
    public Rectangle SourceRectangle { get; set; }
    
    /// <summary>
    /// Gets the width of the sub-section within <see cref="Source"/>.
    /// </summary>
    public int Width => SourceRectangle.Width;

    /// <summary>
    /// Gets the height of the sub-section within <see cref="Source"/>.
    /// </summary>
    public int Height => SourceRectangle.Height;
    
    /// <summary>
    /// Creates a new -- empty -- <see cref="TextureRegion"/>.
    /// </summary>
    public TextureRegion() {  }
    
    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> with specified, but not collected parameters.
    /// </summary>
    /// <param name="source"> The texture to use as the source texture for this <see cref="TextureRegion"/>.</param>
    /// <param name="x"> The x-axis location (in pixels) of the sub-section within <see cref="source"/>.</param>
    /// <param name="y"> The y-axis location (in pixels) of the sub-section within <see cref="source"/>.</param>
    /// <param name="width"> The width (in pixels) of the sub-section within <see cref="source"/>.</param>
    /// <param name="height"> The height (in pixels) of the sub-section within <see cref="source"/>.</param>
    public TextureRegion(Texture2D source, int x, int y, int width, int height)
    {
      Source = source;
      SourceRectangle = new Rectangle(x, y, width, height);
    }
    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> with specified, and halfway collected parameters for the sub-section within a <see cref"Point"/> and an <see cref="Extent"/>.
    /// </summary>
    /// <param name="source"> The texture to use as the source texture for this <see cref="TextureRegion"/>.</param>
    /// <param name="location"> The location (x and y, in pixels) of the sub-section within <see cref="source"/>.</param>
    /// <param name="size"> The size (width and height, in pixels) of the sub-section within <see cref="source"/>.</param>
    public TextureRegion(Texture2D source, Point location, Extent size)
    {
      Source = source;
      SourceRectangle = new Rectangle(location.X, location.Y, size.Width, size.Height);
    }
    
    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> with specified, and fully collected parameters for the sub-section within a <see cref="Rectangle"/>
    /// </summary>
    /// <param name="source"> The texture to use as the source texture for this <see cref="TextureRegion"/>.</param>
    /// <param name="sourceRect"> The location and size (in pixels) of the sub-section within <see cref="source"/>.</param>
    public TextureRegion(Texture2D source, Rectangle sourceRect)
    {
      Source = source;
      SourceRectangle = sourceRect;
    }
    
    /// <summary>
    /// Submit this texture region for drawing in the current batch.
    /// </summary>
    /// <param name="spriteBatch">The spritebatch instance used for batching draw calls.</param>
    /// <param name="position">The xy-coordinate location to draw this texture region on the screen.</param>
    /// <param name="color">The color mask to apply when drawing this texture region on screen.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        Draw(spriteBatch, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
    }

    /// <summary>
    /// Submit this texture region for drawing in the current batch.
    /// </summary>
    /// <param name="spriteBatch">The spritebatch instance used for batching draw calls.</param>
    /// <param name="position">The xy-coordinate location to draw this texture region on the screen.</param>
    /// <param name="color">The color mask to apply when drawing this texture region on screen.</param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when drawing this texture region on screen.</param>
    /// <param name="origin">The center of rotation, scaling, and position when drawing this texture region on screen.</param>
    /// <param name="scale">The scale factor to apply when drawing this texture region on screen.</param>
    /// <param name="effects">Specifies if this texture region should be flipped horizontally, vertically, or both when drawing on screen.</param>
    /// <param name="layerDepth">The depth of the layer to use when drawing this texture region on screen.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
    {
        Draw(
            spriteBatch,
            position,
            color,
            rotation,
            origin,
            new Vector2(scale, scale),
            effects,
            layerDepth
        );
    }

    /// <summary>
    /// Submit this texture region for drawing in the current batch.
    /// </summary>
    /// <param name="spriteBatch">The spritebatch instance used for batching draw calls.</param>
    /// <param name="position">The xy-coordinate location to draw this texture region on the screen.</param>
    /// <param name="color">The color mask to apply when drawing this texture region on screen.</param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when drawing this texture region on screen.</param>
    /// <param name="origin">The center of rotation, scaling, and position when drawing this texture region on screen.</param>
    /// <param name="scale">The amount of scaling to apply to the x- and y-axes when drawing this texture region on screen.</param>
    /// <param name="effects">Specifies if this texture region should be flipped horizontally, vertically, or both when drawing on screen.</param>
    /// <param name="layerDepth">The depth of the layer to use when drawing this texture region on screen.</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        spriteBatch.Draw(
            Source,
            position,
            SourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );
    }
  }
}
