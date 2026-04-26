using UnityEngine;
using TMPro;

namespace AzizStuff
{
    /// <summary>
    /// Reads the live values from HeroStats every frame and pushes them into TMP text fields.
    /// Cached "last value" fields skip string allocs when nothing changed, so leaving this on
    /// always is fine.
    /// </summary>
    public class StatsHUD : MonoBehaviour
    {
        [Tooltip("Source of stats. If left empty, the HUD will FindObjectOfType<HeroStats>() at Awake.")]
        [SerializeField] HeroStats stats;

        [Header("Text Fields (leave null to hide a row)")]
        [SerializeField] TMP_Text damageText;
        [SerializeField] TMP_Text attackSpeedText;
        [SerializeField] TMP_Text maxHealthText;
        [SerializeField] TMP_Text weaponSizeText;

        static readonly string[] _weaponSizeLabels = { "Small", "Medium", "Large" };

        float _lastDamage = float.NaN;
        float _lastAttackSpeed = float.NaN;
        int _lastMaxHealth = int.MinValue;
        int _lastWeaponSize = int.MinValue;

        void Awake()
        {
            if (stats == null) stats = FindObjectOfType<HeroStats>();
        }

        void OnEnable() => Refresh();
        void Update() => Refresh();

        void Refresh()
        {
            if (stats == null) return;

            if (damageText != null && !Mathf.Approximately(_lastDamage, stats.Damage))
            {
                _lastDamage = stats.Damage;
                damageText.text = stats.Damage.ToString("F1");
            }

            if (attackSpeedText != null && !Mathf.Approximately(_lastAttackSpeed, stats.AttackSpeed))
            {
                _lastAttackSpeed = stats.AttackSpeed;
                attackSpeedText.text = stats.AttackSpeed.ToString("F2");
            }

            if (maxHealthText != null && _lastMaxHealth != stats.MaxHealth)
            {
                _lastMaxHealth = stats.MaxHealth;
                maxHealthText.text = stats.MaxHealth.ToString();
            }

            if (weaponSizeText != null && _lastWeaponSize != stats.WeaponSizeLevel)
            {
                _lastWeaponSize = stats.WeaponSizeLevel;
                int idx = Mathf.Clamp(stats.WeaponSizeLevel, 0, _weaponSizeLabels.Length - 1);
                weaponSizeText.text = _weaponSizeLabels[idx];
            }
        }
    }
}
