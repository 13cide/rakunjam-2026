using UnityEngine;
using TMPro;

[RequireComponent(typeof(HeroStats))]
public class Hero : MonoBehaviour, IDamageable
{
    private int currentHealth;

    public HeroStats Stats { get; private set; }

    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        Stats = GetComponent<HeroStats>();
        Stats.OnMaxHealthChanged += HandleMaxHealthChanged;
    }

    private void OnDestroy()
    {
        if (Stats != null)
            Stats.OnMaxHealthChanged -= HandleMaxHealthChanged;
    }

    private void Start()
    {
        currentHealth = Stats.MaxHealth;
        CheckHealth();
    }

    private void HandleMaxHealthChanged(int difference)
    {
        currentHealth += difference;
        CheckHealth();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        CheckHealth();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > Stats.MaxHealth)
        {
            currentHealth = Stats.MaxHealth;
        }
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {Stats.MaxHealth}";
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Hero is dead!");
            // Time.timeScale = 0;
        }
    }
}
