using Opal.Managers;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Opal.Graphics;
using Opal.Params;
using Opal.Hierarchy;

namespace Opal.Tools
{
  public static class GraphicsE
  {
    public static MTexture ToMTexture(this Texture2D texture)
    {
      return new MTexture(texture);
    }

    public static Texture2D ToTexture(this MTexture texture)
    {
      return texture.Texture;
    }

    public static TextureRegion CreateCircle(int radius)
    {
      int diameter = radius * 2;
      Texture2D texture = new Texture2D(Core.Instance.GraphicsDevice, diameter, diameter);
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
      return new TextureRegion(texture, 0, 0, diameter, diameter);
    }

    public static TextureDrawCall Line(Vector2 start, Vector2 end, int thickness)
    {
      Vector2 edge = end - start;

      float angle = MathF.Atan2(edge.Y, edge.X);
      float length = edge.Length();

      var call = new TextureDrawCall
      {
        Texture = Core.Resources.Pixel,
        Params = CanvasParams.Identity with
        {
          Position = start,
          Rotation = angle,
          Scale = new Vector2(length, thickness),
        },
      };

      return call;
    }
  }
}
