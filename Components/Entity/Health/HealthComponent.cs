namespace ConstructEngine.Components.Entity;

public class HealthComponent : Component
{
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public HealthComponent(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
            CurrentHealth = 0;
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
}