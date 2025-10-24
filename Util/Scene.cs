using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;
using ConstructEngine.Components.Entity;
using ConstructEngine.Graphics;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Util;

public class Scene
{
    public bool SceneFrozen { get; set; }

    public static void UpdateEntities(GameTime gameTime)
    {
        for (int i = Entity.EntityList.Count - 1; i >= 0; i--)
        {
            Entity.IEntity e = Entity.EntityList[i];
            e.Update(gameTime);
        }
    }

    public static void InstantiateEntities(string filePath)
    {


        foreach(var entry in EntityLoader.EntityDict)
        {
            Vector2 EntityLoadPosition = entry.Value;

            Entity EntryEntity = entry.Key;



            Entity newEntity = (Entity)Activator.CreateInstance(EntryEntity.GetType());

            Entity.EntityList.Add(newEntity);

            newEntity.KinematicBase.Collider.Rect.X = (int)EntityLoadPosition.X;
            newEntity.KinematicBase.Collider.Rect.Y = (int)EntityLoadPosition.Y;

            newEntity.Load();

        

        }
    }
    
    public static void InstantiateEntities(ContentManager contentManager, string filePath)
    {
        Vector2 LoadPosition = new();
        
        List<Entity> loadedEntities = EntityLoader.LoadEntities(contentManager, filePath);
        List<Vector2> positions = EntityLoader.GetEntityPosition(contentManager, filePath);

        for (int i = 0; i < loadedEntities.Count; i++)
        {
            Type type = loadedEntities[i].GetType();
            Entity newEntity = (Entity)Activator.CreateInstance(type);

            if (i < positions.Count)
            {
                LoadPosition.X = (int)positions[i].X;
                LoadPosition.Y = (int)positions[i].Y;

            }


            Entity.EntityList.Add(newEntity);
        }

        foreach (Entity.IEntity e in Entity.EntityList )
        {
            e.Load();

        }
        
        foreach (Entity e in Entity.EntityList)
        {
            if (e.KinematicBase.Collider.HasRect)
            {
                e.KinematicBase.Collider.Rect.X = (int)LoadPosition.X;
                e.KinematicBase.Collider.Rect.Y = (int)LoadPosition.Y;
            }
            
        }
    }

    public static void DrawEntities(SpriteBatch spriteBatch)
    {
        foreach (Entity.IEntity e in Entity.EntityList)
        {
            e.Draw(spriteBatch);
        }
    }
    
    public interface IScene
    {
        public bool SceneFrozen { get; set; }
        public void Initialize();
        public void Load();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
    
    
    
}