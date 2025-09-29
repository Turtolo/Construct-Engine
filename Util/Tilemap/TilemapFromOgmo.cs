using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using ConstructEngine.Components.Entity;
using ConstructEngine.Graphics;
using ConstructEngine.Object;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Util;

public class TilemapFromOgmo
{
    public static List<Sprite> SpriteList = new List<Sprite>();

    // Helper method

    public static Rectangle GetMapWidth(string filename)
    {
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);
        
        return new Rectangle(0, 0, root.width, root.height);
    }
    
    public static void CreateRectanglesEachTile(string filename, string region)
    {
        string json = File.ReadAllText(filename);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);
        var tileLayer = root.layers.FirstOrDefault(l => l.data != null && l.tileset != null);
    
        if (tileLayer == null)
        {
            throw new Exception("No tile layer found in JSON.");
        }
    
        // Parse region
        string[] split = region.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        int x = int.Parse(split[0]);
        int y = int.Parse(split[1]);
        int width = int.Parse(split[2]);
        int height = int.Parse(split[3]);

        int tileWidth = tileLayer.gridCellWidth;
        int tileHeight = tileLayer.gridCellHeight;

        int columns = tileLayer.gridCellsX;
        int rows = tileLayer.gridCellsY;
        

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                int tilesetIndex = tileLayer.data[index];
                
                if (tilesetIndex == -1) continue;

                int worldX = col * tileWidth + tileLayer.offsetX;
                int worldY = row * tileHeight + tileLayer.offsetY;
                
                
                new Collider(new Rectangle(worldX, worldY, tileWidth, tileHeight), true);
            }
        }
        
    }


    public static void SearchForDecals(string filename)
    {
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);

        
        
        foreach (var layer in root.layers)
        {
            if (layer.folder == null)
            {
                continue;
            }
            

            if (layer.decals == null)
            {
                continue;
            }

            foreach (var decal in layer.decals)
            {
                string path = layer.folder + "/" + decal.texture;
                
                int index = path.IndexOf("Assets", StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    path = path.Substring(index);
                }
                
                Texture2D texture;
                
                
                path = Path.ChangeExtension(path, null);

                texture = Core.Content.Load<Texture2D>(path);
                
                
                
                TextureAtlas atlas = new TextureAtlas(texture);
                
                atlas.AddRegion(path,0,0 , texture.Width, texture.Height);
        
                Sprite sprite = atlas.CreateSprite(path);
                
                
                sprite.StartPosition = new Vector2(decal.x, decal.y);
                
                SpriteList.Add(sprite);
                
                
            }

            
        }
        
    }
    
    
    public static void SearchForObjects(string filename, Entity Player = null, SceneManager sceneManager = null)
    {
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);
        
        foreach (var layer in root.layers)
        {
    
            
            
            if (layer.entities == null)
                continue;

            foreach (var entity in layer.entities)
            {
                

                string className = entity.name;

                if (entity.values == null)
                { 
                    continue;   
                }
                
                Dictionary<string, object> normalDict = new();



                if (entity.values != null)
                {
                    normalDict = entity.values.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.ValueKind switch
                        {
                            JsonValueKind.String => (object)kvp.Value.GetString(),
                            JsonValueKind.Number => kvp.Value.TryGetInt64(out long l) ? l : kvp.Value.GetDouble(),
                            JsonValueKind.True => true,
                            JsonValueKind.False => false,
                            JsonValueKind.Null => null!,
                            JsonValueKind.Object => JsonSerializer.Deserialize<Dictionary<string, object>>(
                                kvp.Value.GetRawText()),
                            JsonValueKind.Array => JsonSerializer.Deserialize<List<object>>(kvp.Value.GetRawText()),
                            _ => kvp.Value.GetRawText()
                        }
                    ); 
                }


                string name = "ConstructObject";
                
                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetTypes().Any(t => t.Name == className && typeof(ConstructObject).IsAssignableFrom(t)));


                if (assembly != null)
                {
                    Type type = assembly.GetTypes()
                        .First(t => t.Name == className && typeof(ConstructObject).IsAssignableFrom(t));

                    ConstructObject ObjectClass = (ConstructObject)Activator.CreateInstance(type);
                    
                    ObjectClass.Rectangle = new(entity.x, entity.y, entity.width, entity.height);

                    ObjectClass.Name = entity.name;
                    
                    ObjectClass.Values = normalDict;
                    

                    if (Player != null)
                    {
                        ObjectClass.Player = Player;
                    }

                    if (sceneManager != null)
                    {
                        ObjectClass.CurrentSceneManager = sceneManager;
                    }
                    
                    LoadObjects();
                    
                }
            }
        }
    }


    public static void LoadObjects()
    {
        for (int i = 0; i < ConstructObject.ObjectList.Count; i++)
        {
            ConstructObject.ObjectList[i].Load();
        }
    }


    public static void DrawObjects(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < ConstructObject.ObjectList.Count; i++)
        {
            ConstructObject.ObjectList[i].Draw(spriteBatch);
        }
    }
    
    public static void DrawDecals(string filename, SpriteBatch spriteBatch)
    {
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);

        Texture2D texture;
        
        foreach (var layer in root.layers)
        {
            if (layer.folder == null)
            {
                continue;
            }
            

            if (layer.decals == null)
            {
                continue;
            }
            
            foreach (Sprite sprite in SpriteList) 
            {
                sprite.Draw(spriteBatch, sprite.StartPosition);
                
            }

            

            
        }
        
    }


    
    

    public static void UpdateObjects(GameTime gameTime)
    {   
        for (int i = 0; i < ConstructObject.ObjectList.Count; i++)
        {
            ConstructObject.ObjectList[i].Update(gameTime);
        }
    }
    
    
    
    public static void GetCollisions(ContentManager content, string filename)
    {
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);

        foreach (var layer in root.layers)
        {
            if (layer.entities == null)
                continue;

            foreach (var entity in layer.entities)
            {
                if (entity.values != null &&
                    entity.values.TryGetValue("collision", out JsonElement element) &&
                    element.ValueKind == JsonValueKind.True)
                {
                    int width = entity.width;
                    int height = entity.height;

                    new Collider(new Rectangle(entity.x, entity.y, width, height), true);
                }
                
            }
        }
    }
    

   
    public static void FromFile(ContentManager content, string filename, string region, string textureName = null)
    {
        string json = File.ReadAllText(filename);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);
        var tileLayer = root.layers.FirstOrDefault(l => l.data != null && l.tileset != null);
        
        if (tileLayer == null)
        {
            throw new Exception("No tile layer found in JSON.");
        }
        
        string[] split = region.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        int x = int.Parse(split[0]);
        int y = int.Parse(split[1]);
        int width = int.Parse(split[2]);
        int height = int.Parse(split[3]);

        foreach (var layer in root.layers)
        {
            if (layer.tileset == null)
            {
                continue;
            }

            string contPath = "";

            if (textureName != null)
            {
                contPath = textureName;
            }

            else
            {
                contPath = layer.texturePath;
            }

            int tileWidth = layer.gridCellWidth;
            int tileHeight = layer.gridCellHeight;

            Texture2D texture = content.Load<Texture2D>(contPath);
            TextureRegion textureRegion = new TextureRegion(texture, x, y, width, height);
            Tileset tileset = new Tileset(textureRegion, tileWidth, tileHeight);

            int columns = layer.gridCellsX;
            int rows = layer.gridCellsY;

            Tilemap tilemap = new Tilemap(tileset, columns, rows, 0.1f);


            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;
                    
                    if (index >= 0 && index < layer.data.Count)
                    {
                        int tilesetIndex = layer.data[index];

                        if (tilesetIndex == -1)
                        {
                            tilesetIndex = 0;
                        }

                        tilemap.SetTile(col, row, tilesetIndex);
                    }
                }
            }

            
            tilemap.LayerDepth = float.Parse(layer.name, CultureInfo.InvariantCulture);

            Tilemap.Tilemaps.Add(tilemap);
        }

    }
    

    
}