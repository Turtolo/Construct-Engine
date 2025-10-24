using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Xna.Framework;
using ConstructEngine.Components;
using ConstructEngine.Components.Entity;
using ConstructEngine.Object;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Util;

public class EntityLoader
{
    public static Dictionary<Entity, Vector2> GetEntityData(string filename)
    {
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);

        Dictionary<Entity, Vector2> EntityDict = new();

        foreach (var layer in root.layers)
        {
            if (layer.entities == null)
            {
                continue;
            }

            foreach (var entity in layer.entities)
            {
                string className = entity.name;

                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetTypes().Any(t => t.Name == className && typeof(Entity).IsAssignableFrom(t)));

                if (assembly != null)
                {
                    Type type = assembly.GetTypes().First(t => t.Name == className && typeof(Entity).IsAssignableFrom(t));

                    Entity EntityInstance = (Entity)Activator.CreateInstance(type);

                    Vector2 EntityPosition = new Vector2(entity.x, entity.y);

                    EntityDict.Add(EntityInstance, EntityPosition);

                }
            }
        }

        return EntityDict;
    }
    
    

}