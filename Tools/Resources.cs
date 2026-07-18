using System;
using Opal.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Opal.Tools
{
  public class Resources : BaseObject
  {
    /// <summary>
    /// Gets The internal <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <remarks>
    /// Due to engine abstraction, this will only be used in cases where you use only some helpers from the enging. And are not using systems such as tokens.
    /// </remarks>
    public SpriteBatch SpriteBatch { get; private set; } = new SpriteBatch(Core.Instance.GraphicsDevice);

    /// <summary>
    /// A default, bitmap-font, with pixel characters.
    /// </summary>
    /// <remarks>
    /// This could be used for most tasks, but for a bit of "flare", you should use your own font.
    /// </remarks>
    public BitmapFont BitmapFont { get; private set; }

    /// <summary>
    /// A single 1x1 pixel, useful for most graphics in situations where you may not be able to load a resource.
    /// </summary>
    public MTexture Pixel { get; private set; } = new MTexture(1, 1, new[] { Color.White });

    internal void LoadContent()
    {
      var assembly = typeof(Core).Assembly;
      using var stream = assembly.GetManifestResourceStream("Opal.Graphics.Font.bitmap_font.png");
      if (stream == null)
        throw new InvalidOperationException("Embedded resource not found: Opal.Graphics.Font.bitmap_font.png");

      var texture = Texture2D.FromStream(Core.Instance.GraphicsDevice, stream);
      BitmapFont = new BitmapFont(texture, 6, 10);
      BitmapFont.AddMap("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-=()[]{}<>/*:#%!?.,'\"@&$");
    }

    internal void UnloadContent()
    {
      BitmapFont.Texture.Dispose();
    }
  }
}
