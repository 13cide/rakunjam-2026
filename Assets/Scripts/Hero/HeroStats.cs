using UnityEngine;

public class HeroStats : MonoBehaviour
{
    [Header("Health")]
    public int BaseMaxHealth = 100;
    public int AdditionalMaxHealth = 0;
    public int MaxHealth => BaseMaxHealth + AdditionalMaxHealth;

    [Header("Damage")]
    public float BaseDamage = 10f;
    public float DamageMultiplier = 1f; 
    public float Damage => BaseDamage * DamageMultiplier;

    [Header("Attack Speed")]
    public float BaseAttackSpeed = 1f;
    public float AttackSpeedMultiplier = 1f;
    public float AttackSpeed => BaseAttackSpeed * AttackSpeedMultiplier;

    [Header("Weapon Size")]
    [Tooltip("0 = Small, 1 = Medium, 2 = Large")]
    [Range(0, 2)]
    public int WeaponSizeLevel = 0;

    public event System.Action<int> OnWeaponSizeChanged;
    public event System.Action<int> OnMaxHealthChanged;

    public void AddDamageBuff(float percent) => DamageMultiplier += percent;
    public void RemoveDamageBuff(float percent) => DamageMultiplier -= percent;

    public void AddAttackSpeedBuff(float percent) => AttackSpeedMultiplier += percent;
    public void RemoveAttackSpeedBuff(float percent) => AttackSpeedMultiplier -= percent;

    public void AddMaxHealthBuff(int amount)
    {
        int oldMax = MaxHealth;
        AdditionalMaxHealth += amount;
        int newMax = MaxHealth;
        OnMaxHealthChanged?.Invoke(newMax - oldMax);
    }

    public void RemoveMaxHealthBuff(int amount)
    {
        int oldMax = MaxHealth;
        AdditionalMaxHealth -= amount;
        int newMax = MaxHealth;
        OnMaxHealthChanged?.Invoke(newMax - oldMax);
    }

    public void IncreaseWeaponSize()
    {
        if (WeaponSizeLevel < 2)
        {
            WeaponSizeLevel++;
            OnWeaponSizeChanged?.Invoke(WeaponSizeLevel);
        }
    }
}
