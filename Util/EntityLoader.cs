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

    public static Dictionary<Entity, Vector2> EntityDict = new();

    public static void GetEntityData(string filename)
    {
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
                string className = entity.name;

                Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetTypes().Any(t => t.Name == className && typeof(Entity).IsAssignableFrom(t)));

                if (assembly != null)
                {
                    Type type = assembly.GetTypes().First(t => t.Name == className && typeof(Entity).IsAssignableFrom(t));

                    Entity EntityInstance = (Entity)Activator.CreateInstance(type);

                    Console.WriteLine(EntityInstance.GetType());

                    Vector2 EntityPosition = new Vector2(entity.x, entity.y);

                    EntityDict.Add(EntityInstance, EntityPosition);

                }
            }
        }
    }
    
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
                
                string className = entity.name;
                
                Assembly assembly = Assembly.GetExecutingAssembly();

                
                Type type = assembly.GetTypes().FirstOrDefault(t => t.Name == className);

                if (type == null)
                {
                    positionList.Add(new Vector2(entity.x, entity.y));
                    
                }
            }
        }
        
        
        return positionList;
    }

}