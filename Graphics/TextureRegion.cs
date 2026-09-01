using System;
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
    /// Gets the size of the sub-section within <see cref="Source"/>.
    /// </summary>
    public Extent Size => new Extent(Width, Height);

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
    /// Creates a new <see cref="TextureRegion"/> sub-section within this one. This can be useful for tilesets, where you need to split it into the map, and then tiles.
    /// </summary>
    /// <remarks>
    /// Clamps the values so that they are not negative, nor surpass the <see cref="Size"/> of this <see cref="TextureRegion"/>
    /// </remarks>
    /// <param name="x"> The x-axis location (in pixels) of the sub-section within <see cref="SourceRectangle"/>.</param>
    /// <param name="y"> The y-axis location (in pixels) of the sub-section within <see cref="SourceRectangle"/>.</param>
    /// <param name="width"> The width (in pixels) of the sub-section within <see cref="SourceRectangle"/>.</param>
    /// <param name="height"> The height (in pixels) of the sub-section within <see cref="SourceRectangle"/>.</param>
    public TextureRegion CreateSubRegion(int x, int y, int width, int height)
    {
      x = Math.Max(0, x);
      y = Math.Max(0, y);

      width = Math.Min(width, Width - x);
      height = Math.Min(height, Height - y);

      int absoluteX = SourceRectangle.X + x;
      int absoluteY = SourceRectangle.Y + y;

      return new TextureRegion(Source, absoluteX, absoluteY, width, height);
    }

    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> sub-section within this one. This can be useful for tilesets, where you need to split it into the map, and then tiles.
    /// </summary>
    /// <remarks>
    /// Clamps the values so that they are not negative, nor surpass the <see cref="Size"/> of this <see cref="TextureRegion"/>
    /// </remarks>
    /// <param name="location"> The location (in pixels) of the sub-section within <see cref="SourceRectangle"/>.</param>
    /// <param name="size"> The size (in pixels) of the sub-section within <see cref="SourceRectangle"/>.</param>
    public TextureRegion CreateSubRegion(Point location, Extent size)
    {
      return CreateSubRegion(location.X, location.Y, size.Width, size.Height);
    }

    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> sub-section within this one. This can be useful for tilesets, where you need to split it into the map, and then tiles.
    /// </summary>
    /// <remarks>
    /// Clamps the values so that they are not negative, nor surpass the <see cref="Size"/> of this <see cref="TextureRegion"/>
    /// </remarks>
    /// <param name="sourceRect"> The source rectangle, represents the size of the sub-section within <see cref="SourceRectangle"/>.</param>
    public TextureRegion CreateSubRegion(Rectangle sourceRect)
    {
      return CreateSubRegion(sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
    }
  }
}
