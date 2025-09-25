using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.Xna.Framework;
using ConstructEngine.Components;
using ConstructEngine.Components.Entity;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Util;

public class EntityLoader
{

    
    public static List<Entity> LoadEntities(ContentManager content, string filename)
    {
        List <Entity> entities = new List<Entity>();
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
                    
                    
                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetTypes().Any(t => t.Name == className && typeof(Entity).IsAssignableFrom(t)));


                if (assembly != null)
                {


                    Type type = assembly.GetTypes()
                        .First(t => t.Name == className && typeof(Entity).IsAssignableFrom(t));



                    Entity instance = (Entity)Activator.CreateInstance(type);
                    

                    entities.Add(instance);
                }
            }
        }
        
        return entities;
    }
    
    
    public static List<Vector2> GetEntityPosition(ContentManager content, string filename)
    {
        List<Vector2> positionList = new List<Vector2>();
        
        string json = File.ReadAllText(filename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        OgmoReader.Root root = JsonSerializer.Deserialize<OgmoReader.Root>(json, options);

        foreach (var layer in root.layers)
        {
            if (layer.entities == null)
            {
                continue;
            }

            foreach (var entity in layer.entities)
            {
                if (entity.name != "collision" && entity.name != "SceneTransitionArea")
                {
                    positionList.Add(new Vector2(entity.x, entity.y));
                    
                }
            }
        }
        
        
        return positionList;
    }

}