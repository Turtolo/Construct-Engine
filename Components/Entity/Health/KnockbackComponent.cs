using System;
using Microsoft.Xna.Framework;
using ConstructEngine.Components.Entity;

namespace ConstructEngine.Components;

public class KnockbackComponent : Component
{
    public Vector2 KnockbackForce { get; set; }
    public Entity.Entity HostEntity { get; set; }

    public KnockbackComponent(int xForce, int yForce, object root, Entity.Entity entity) : base(root)
    {
        KnockbackForce = new(xForce, yForce);
    }

    public void ApplyKnocback()
    {

    }
}
