using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConstructEngine.Components
{
    public class BackseatComponent : Component
    {
        public static List<BackseatComponent> BackseatComponentList = new();

        public BackseatComponent(object root) : base(root)
        {
            BackseatComponentList.Add(this);
        }
        
        public virtual void Load() {}
        
        public virtual void Update(GameTime gameTime) {}
        
        public virtual void Draw(SpriteBatch spriteBatch) {}

    }
}