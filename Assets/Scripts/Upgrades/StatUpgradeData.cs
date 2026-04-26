using UnityEngine;

public enum StatType
{
    Damage,
    AttackSpeed,
    WeaponSize,
    MaxHealth,
    Heal
}

[CreateAssetMenu(menuName = "Upgrades/Stat Upgrade")]
public class StatUpgradeData : UpgradeData
{
    [Header("Stat Modifier")]
    public StatType StatToUpgrade;
    
    [Tooltip("The amount to increase. Percentage for Damage/AttackSpeed (e.g. 0.1 = 10%), Flat amount for Health/Heal.")]
    public float Value = 0.1f;

    public override void Apply(Hero hero)
    {
        switch (StatToUpgrade)
        {
            case StatType.Damage:
                hero.Stats.AddDamageBuff(Value);
                break;
            case StatType.AttackSpeed:
                hero.Stats.AddAttackSpeedBuff(Value);
                break;
            case StatType.WeaponSize:
                hero.Stats.IncreaseWeaponSize();
                break;
            case StatType.MaxHealth:
                hero.Stats.AddMaxHealthBuff((int)Value);
                break;
            case StatType.Heal:
                hero.Heal((int)Value);
                break;
        }
        
        if (StatToUpgrade == StatType.WeaponSize)
        {
            Debug.Log($"Applied Upgrade: {UpgradeName} (Tier {Tier}) - Increased {StatToUpgrade} level.");
        }
        else if (StatToUpgrade == StatType.MaxHealth)
        {
            Debug.Log($"Applied Upgrade: {UpgradeName} (Tier {Tier}) - Increased {StatToUpgrade} by {(int)Value}");
        }
        else if (StatToUpgrade == StatType.Heal)
        {
            Debug.Log($"Applied Upgrade: {UpgradeName} (Tier {Tier}) - Healed for {(int)Value}");
        }
        else
        {
            Debug.Log($"Applied Upgrade: {UpgradeName} (Tier {Tier}) - Increased {StatToUpgrade} by {Value * 100}%");
        }
    }
}
