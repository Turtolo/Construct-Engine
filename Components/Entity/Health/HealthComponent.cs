using System;
using System.Linq;
using ConstructEngine.Area;
using ConstructEngine.Util;
using ConstructEngine;
using Microsoft.Xna.Framework;
using Timer = ConstructEngine.Util.Timer;

namespace ConstructEngine.Components.Entity;

public class HealthComponent : Component
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public float HitFreezeDuration { get; set; }

    public Area2D TakeDamageArea { get; set; }
    private Entity HostEntity { get; set; }
    public bool CanTakeDamage { get; set; } = true;


    public HealthComponent(Entity entity, int maxHealth, Area2D takeDamageArea, object root) : base(root)
    {
        TakeDamageArea = takeDamageArea;
        HostEntity = entity;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        HitFreezeDuration = 0.1f;
    }

    public HealthComponent(Entity entity, int maxHealth, Area2D takeDamageArea, object root, float hitFreezeDuration) : base(root)
    {
        TakeDamageArea = takeDamageArea;
        HostEntity = entity;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        HitFreezeDuration = hitFreezeDuration;
    }

    public void TakeDamage(int damage)
    {
        if (!CanTakeDamage) return;

        Core.SceneManager.QueeFreezeCurrentSceneFor(0.5f);

        CanTakeDamage = false;
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        Timer.Wait(1.0f, () => { CanTakeDamage = true; });
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
            var otherRoot = TakeDamageArea.GetCurrentlyIntersectingArea().Root;
            if (otherRoot is Entity otherEntity)
            {
                if (otherEntity.GetType() != RootType)
                    TakeDamage(otherEntity.DamageAmount);
            }
        }

        if (CurrentHealth <= 0)
        {
            HostEntity.Free();
        }
    }
}
