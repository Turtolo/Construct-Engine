using ConstructEngine.Components.Physics;
using System;
using System.Collections.Generic;
using ConstructEngine.Graphics;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Components.Entity
{
    public class Entity : Entity.IEntity
    {
        public static List<Entity> EntityList = new List<Entity>();

        public KinematicBase KinematicBase;

        public Sprite Sprite { get; set; } = null;
        public AnimatedSprite AnimatedSprite { get; set; } = null;
        public AnimatedSprite AnimatedSpriteFeet { get; set; } = null;

        public string String;
        

        
        

        public Entity()
        {
            
            KinematicBase = new KinematicBase();
            
        }
        
        public interface IEntity
        {
            public Sprite Sprite { get; set; }
            public AnimatedSprite AnimatedSprite { get; set; }
            void Load();
            void Update(GameTime gameTime);
            void Draw(SpriteBatch spriteBatch);
        }
        
        public virtual void Load()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public static void AddEntities(params Entity[] entities)
        {
            EntityList.AddRange(entities);
        }
        
    }
}