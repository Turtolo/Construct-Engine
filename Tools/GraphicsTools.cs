using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Amethyst.Graphics;
using Amethyst.Params;

namespace Amethyst.Tools
{
  public static class GraphicsE
  {
    public static MTexture LoadBaseObject64Premultiplied(this string base64String)
    {
      if (base64String.Contains(",")) base64String = base64String.Split(',')[1];
      byte[] imageBytes = Convert.FromBase64String(base64String);

      Texture2D texture;

      using (MemoryStream ms = new MemoryStream(imageBytes))
      {
        texture = Texture2D.FromStream(Core.GraphicsDevice, ms);
      }

      Color[] pixels = new Color[texture.Width * texture.Height];
      texture.GetData(pixels);

      for (int i = 0; i < pixels.Length; i++)
      {
        Color p = pixels[i];
        float alpha = p.A / 255f;

        pixels[i] = new Color(
            (byte)(p.R * alpha),
            (byte)(p.G * alpha),
            (byte)(p.B * alpha),
            p.A
        );
      }

      texture.SetData(pixels);

      return texture.ToMTexture();
    }

    public static MTexture ToMTexture(this Texture2D texture)
    {
      return new MTexture(texture);
    }

    public static Texture2D ToTexture(this MTexture texture)
    {
      return texture.Texture;
    }

    public static MTexture CreateCircle(int radius)
    {
      int diameter = radius * 2;
      Texture2D texture = new Texture2D(Core.GraphicsDevice, diameter, diameter);
      Color[] data = new Color[diameter * diameter];

      for (int y = 0; y < diameter; y++)
      {
        for (int x = 0; x < diameter; x++)
        {
          int index = x + y * diameter;
          Vector2 pos = new Vector2(x - radius, y - radius);

          if (pos.Length() <= radius)
            data[index] = Color.White;
          else
            data[index] = Color.Transparent;
        }
      }

      texture.SetData(data);
      return texture.ToMTexture();
    }

    public static TextureDrawCall Line(Vector2 start, Vector2 end, Color color, int thickness)
    {
      Vector2 edge = start - end;

      float angle = MathF.Atan2(edge.Y, edge.X);
      float length = edge.Length();

      var call = new TextureDrawCall
      {
        Texture = Core.Pixel,
        Params = CanvasParams.Identity with
        {
          Position = start,
          Color = color,
          Rotation = angle,
          Scale = new Vector2(length, thickness)
        }
      };

      return call;
    }
  }
}
