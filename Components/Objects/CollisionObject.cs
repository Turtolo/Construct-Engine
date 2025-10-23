using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using ConstructEngine.Object;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Objects;

public class CollisionObject : ConstructObject, ConstructObject.IObject
{
    
    public bool Collidable { get; set; }
    
    public bool OneWay { get; set; }
    
    public Collider Collider { get; set; }

    public CollisionObject()
    {

    }
    
    public CollisionObject(Rectangle rect, bool collidable, bool oneway)
    {
        OneWay = oneway;
        Collidable = collidable;
        Rectangle = rect;
        Collider = new Collider(Rectangle, Collidable, this);
        
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

        Collider = new Collider(Rectangle, Collidable, this);




    }

    public override void Update(GameTime gameTime)
    {

        if (OneWay)
        {
            bool playerAbove = Player.KinematicBase.Collider.Rect.Bottom <= Collider.Rect.Top;
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