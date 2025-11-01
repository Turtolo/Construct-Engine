using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using ConstructEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Directory;

public class TilemapFromTiled
{
    public static void GetObjects(ContentManager content, string filename)
    {
        string json =  File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        TiledReader.TiledMap root = JsonSerializer.Deserialize<TiledReader.TiledMap>(json, options);

        foreach (var layer in root.layers)
        {
            if (layer.type != "objectgroup" || layer.objects == null)
                continue;

            foreach (var obj in layer.objects)
            {
                bool isSolid = false;
                
                if (obj.properties != null)
                {
                    var collisionProp = obj.properties.FirstOrDefault(p => p.name == "collision");
                    if (collisionProp != null && collisionProp.value is bool val && val)
                    {
                        isSolid = true;
                    }
                }

                if (isSolid)
                {
                    int width = (int)obj.width;
                    int height = (int)obj.height;
                }
            }
        }
    }


    public static Tilemap FromFile(ContentManager content, string filename, string contentPath, string region, float layerDepth)
    {
        string json =  File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        TiledReader.TiledMap root = JsonSerializer.Deserialize<TiledReader.TiledMap>(json, options);
        
        var tileLayer = root.layers.FirstOrDefault(l => l.data != null);
        if (tileLayer == null)
        {
            throw new Exception("No tile layer found in JSON");
        }
        
        string[] split = region.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        int x = int.Parse(split[0]);
        int y = int.Parse(split[1]);
        int width = int.Parse(split[2]);
        int height = int.Parse(split[3]);

        int tileWidth = root.tilewidth;
        int tileHeight = root.tileheight;
        
        Texture2D texture = content.Load<Texture2D>(contentPath);
        TextureRegion textureRegion = new TextureRegion(texture, x, y, width, height);
        Tileset tileset = new Tileset(textureRegion, tileWidth, tileHeight);
        
        int columns = tileLayer.width;
        int rows = tileLayer.height;
        
        Tilemap tilemap = new Tilemap(tileset, columns, rows, layerDepth);
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                int tilesetIndex = tileLayer.data[index];

                if (tilesetIndex == -1)
                {
                    tilesetIndex = -tilesetIndex;
                }
                
                tilemap.SetTile(col, row, tilesetIndex);
            }
        }

        return tilemap;


    }

}