using ConstructEngine.Components.Physics;
using System;
using System.Collections.Generic;
using ConstructEngine.Graphics;
using ConstructEngine.Area;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConstructEngine.Directory;

namespace ConstructEngine.Components.Entity
{
    public class Entity : Entity.IEntity
    {
        public static List<Entity> EntityList = new List<Entity>();

        public static Dictionary<Entity, int> EntityDamageDict = new();
        public KinematicBase KinematicBase;
        public Sprite Sprite { get; set; } = null;
        public AnimatedSprite AnimatedSprite { get; set; } = null;
        public AnimatedSprite AnimatedSpriteFeet { get; set; } = null;
        public bool Visible { get; set; } = true;
        
        public int DamageAmount { get; set; }

        public string String;

        public Entity(int damageAmount)
        {
            DamageAmount = damageAmount;
            EntityDamageDict.Add(this, DamageAmount);
            KinematicBase = new KinematicBase();
        }

        /// <summary>
        /// The interface for entity, includes a sprite, an animated sprite and basic functions.
        /// </summary>
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

        /// <summary>
        /// Draws both the animated and normal sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="textureOffset"></param>

        public void DrawSprites(SpriteBatch spriteBatch, Vector2 position, int textureOffset = 0)
        {
            if (Visible)
            {
                if (AnimatedSprite != null)
                {
                    AnimatedSprite.Draw(spriteBatch, new Vector2(position.X + textureOffset, position.Y));
                }

                if (AnimatedSpriteFeet != null)
                {
                    AnimatedSpriteFeet.Draw(spriteBatch, new Vector2(position.X + textureOffset, position.Y));
                }
            }
        }

        /// <summary>
        /// Frees the entity immediately
        /// </summary>

        public void Free()
        {
            EntityList.Remove(this);
            KinematicBase.Collider.Free();
        }
        
        /// <summary>
        /// A function for adding multiple entities at once
        /// </summary>
        /// <param name="entities"></param>

        public static void AddEntities(params Entity[] entities)
        {
            EntityList.AddRange(entities);
        }
        
    }
}