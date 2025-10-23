using System;
using ConstructEngine.Physics;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Components.Entity;

public class DealDamageComponent : Component
{
    public int DamageAmount { get; set; }

    public Collider DamageArea;

    public HealthComponent HealthComponent;

    public DealDamageComponent(Collider area, int damageAmount)
    {
        DamageArea = area;
        DamageAmount = damageAmount;
    }
    

    public void Update(GameTime gameTime)
    {
        if (DamageArea.IsIntersectingAny())
        {
            if (DamageArea.GetCurrentlyIntersectingCollider().Root is Entity)
            {
                HealthComponent?.TakeDamage(DamageAmount);
            }
        }
    }
}