using System;
using System.ComponentModel;
using ConstructEngine.Object;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Objects;

public class ColliderObject : ConstructObject, ConstructObject.IObject
{
    
    public bool Collidable { get; set; }
    
    public bool OneWay { get; set; }
    
    public Collider Collider { get; set; }
    
    public ColliderObject()
    {
        
    }

    public override void Load()
    {
        if (Values.ContainsKey("Collision"))
        {
            if (Values["Collision"] as bool? == true)
            {
                Collidable = true;
            }
        }

        if (Values.ContainsKey("OneWay"))
        {
            if (Values["OneWay"] as bool? == true)
            {
                OneWay = true;
                Collidable = false;
            }
        }
        
        Collider = new Collider(Rectangle, Collidable, OneWay, null);
    }

    public override void Update(GameTime gameTime)
    {

        if (OneWay)
        {
            bool playerAbove = Player.KinematicBase.Hitbox.Bottom <= Collider.Rect.Top;
            bool movingDown = Player.KinematicBase.Velocity.Y >= 0;

            if (playerAbove && movingDown)
            {
                Collider.Enabled = true;
            }
            else
            {
                Collider.Enabled = false;
            }
        }
    }
}