using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Opal.Graphics
{
  public class TextureAtlas : BaseObject
  { 
    private Dictionary<string, TextureRegion> _regions;
    
    /// <summary>
    /// Gets or Sets the source used by all texture regions within <see cref="Regions"/>.
    /// </summary>
    /// <remarks>
    /// For more documentation, see <see cref="TextureRegion.Source"/>.
    /// </remarks>
    public Texture2D Source { get; set; }
    
    /// <summary>
    /// Creates a new, empty, <see cref="TextureAtlas"/>.
    /// </summary>
    public TextureAtlas()
    {
      _regions = new Dictionary<string, TextureRegion>();
    }

    /// <summary>
    /// Creates a new <see cref="TextureAtlas"/> with a provided source.
    /// </summary>
    /// <param name="source"> The source which all <see cref="TextureRegion"/>s within this <see cref="TextureAtlas"/> will use </param>
    public TextureAtlas(Texture2D source)
    {
      Source = source;
      _regions = new Dictionary<string, TextureRegion>();
    }
    
    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="TextureAtlas"/>
    /// </summary>
    /// <param name="name"> The name of the region, used later with <see cref="GetRegion(string)"/>.</param>
    /// <param name="x">The top-left x-coordinate position of the region boundary relative to the top-left corner of the source texture boundary.</param>
    /// <param name="y">The top-left y-coordinate position of the region boundary relative to the top-left corner of the source texture boundary.</param>
    /// <param name="width">The width, in pixels, of the region.</param>
    /// <param name="height">The height, in pixels, of the region.</param>
    public void AddRegion(string name, int x, int y, int width, int height)
    {
      TextureRegion region = new TextureRegion(Source, x, y, width, height);
      _regions.Add(name, region);
    }
    /// <summary>
    /// Gets the <see cref="TextureRegion"/> from this <see cref="TextureAtlas"> using a specified name.
    /// </summary>
    /// <param name="name"> The name which will be used for lookup.</param>
    public TextureRegion GetRegion(string name)
    {
      return _regions[name];
    }

    public TextureRegion[] GetAllRegions()
    {
      TextureRegion[] valuesArray = new TextureRegion[_regions.Count];
      _regions.Values.CopyTo(valuesArray, 0);

      return valuesArray;
    }
    
    /// <summary>
    /// Removes a <see cref="TextureRegion"/> from this atlas.
    /// </summary>
    /// <remarks>
    /// Returns true if successful, false if not.
    /// </remarks>
    /// <param name="name"> The name of the region which wil be removed </param>.
    public bool RemoveRegion(string name)
    {
      return _regions.Remove(name);
    }

    /// <summary>
    /// Removes all regions from this altas.
    /// </summary>
    /// <remarks>
    /// Does not remove <see cref="Source"/>.
    /// </remarks>
    public void Clear()
    {
      _regions.Clear();
    }

    public static TextureAtlas FromFile(string fileName)
    {
      TextureAtlas textureAtlas = new TextureAtlas();
      string filePath = Path.Combine(Core.Resource.ContentRoot, fileName);

      using (Stream stream = TitleContainer.OpenStream(filePath))
      {
        using (XmlReader reader = XmlReader.Create(stream))
        {

        }
      }

      return textureAtlas;
    }
  }
}
