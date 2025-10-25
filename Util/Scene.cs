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
    

    public static void DrawEntities(SpriteBatch spriteBatch)
    {
        foreach (Entity.IEntity e in Entity.EntityList)
        {
            e.Draw(spriteBatch);
        }
    }
    
    public interface IScene
    {
        public void Initialize();
        public void Load();
        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);

    }
    
    
    
}