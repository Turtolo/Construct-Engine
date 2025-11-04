using System;
using System.Linq;
using ConstructEngine.Area;
using Microsoft.Xna.Framework;
using Timer = ConstructEngine.Util.Timer;

namespace ConstructEngine.Components.Entity;

public class HealthComponent : Component
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public Area2D TakeDamageArea { get; set; }
    private Entity HostEntity { get; set; }

    public bool CanTakeDamage { get; set; } = true;

    public HealthComponent(Entity entity, int maxHealth)
    {
        HostEntity = entity;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public HealthComponent(Entity entity, int maxHealth, Area2D takeDamageArea)
    {
        TakeDamageArea = takeDamageArea;
        HostEntity = entity;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CanTakeDamage = false;
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        Timer.Wait(1.0f, () => {CanTakeDamage = true;});
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    public void Update(GameTime gameTime)
    {
        if (TakeDamageArea.IsIntersectingAny())
        {
            var rootEntity = TakeDamageArea.GetCurrentlyIntersectingArea()?.Root as Entity;

            if (rootEntity != null && Entity.EntityDamageDict.TryGetValue(rootEntity, out int damageAmount))
            {
                if (CanTakeDamage) TakeDamage(damageAmount);
            }
        }

        if (CurrentHealth <= 0)
        {
            HostEntity.Free();
        }
    }
}