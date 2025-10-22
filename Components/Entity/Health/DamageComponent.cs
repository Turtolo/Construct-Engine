using ConstructEngine.Physics;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Components.Entity;

public class DamageComponent : Component
{
    public int DamageAmount { get; set; }

    public Collider DamageArea;

    public DamageComponent(Collider area, int damageAmount)
    {
        DamageArea = area;
        DamageAmount = damageAmount;
    }
    

    public void Update(GameTime gameTime)
    {
    
    }
}