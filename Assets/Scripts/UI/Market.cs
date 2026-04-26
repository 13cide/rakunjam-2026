using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Market : MonoBehaviour
{

    [Header("Tier1")]
    [SerializeField] private TMP_Text Tier1CostText;
    [SerializeField] private List<UpgradeData> Tier1Upgrades = new List<UpgradeData>();

    [Header("Tier2")]
    [SerializeField] private TMP_Text Tier2CostText;
    [SerializeField] private List<UpgradeData> Tier2Upgrades = new List<UpgradeData>();

    [Header("Tier3")]
    [SerializeField] private TMP_Text Tier3CostText;
    [SerializeField] private List<UpgradeData> Tier3Upgrades = new List<UpgradeData>();

    private List<UpgradeData> tier1Pool;
    private List<UpgradeData> tier2Pool;
    private List<UpgradeData> tier3Pool;

    [Header("References")]
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private Rullete rullete;
    [SerializeField] private Animator marketAnimator;
    [SerializeField] private Animator rulleteAnimator;

    [Header("Costs")]
    public int[] TierCosts = {10, 20, 30};
    [SerializeField] private float costMultiplier = 1.3f;

    [Header("Tier Colors")]
    [SerializeField] private Color tier1Color = Color.white;
    [SerializeField] private Color tier2Color = Color.yellow;
    [SerializeField] private Color tier3Color = new Color(1f, 0.5f, 0f); // Orange

    public void BuyUpgrade(int tier)
    {
        if (tier < 1 || tier > 3) return;

        int cost = TierCosts[tier - 1];
        
        if (moneyManager != null && moneyManager.TrySpendMoney(cost))
        {
            TierCosts[tier - 1] = Mathf.RoundToInt(cost * costMultiplier);
            
            List<UpgradeData> sourceList = null;
            List<UpgradeData> currentPool = null;
            Color tierColor = Color.white;

            if (tier1Pool == null) tier1Pool = new List<UpgradeData>();
            if (tier2Pool == null) tier2Pool = new List<UpgradeData>();
            if (tier3Pool == null) tier3Pool = new List<UpgradeData>();

            switch (tier)
            {
                case 1: 
                    sourceList = Tier1Upgrades; 
                    currentPool = tier1Pool;
                    tierColor = tier1Color; 
                    Tier1CostText.text = TierCosts[tier - 1].ToString(); 
                    break;
                case 2: 
                    sourceList = Tier2Upgrades; 
                    currentPool = tier2Pool;
                    tierColor = tier2Color; 
                    Tier2CostText.text = TierCosts[tier - 1].ToString(); 
                    break;
                case 3: 
                    sourceList = Tier3Upgrades; 
                    currentPool = tier3Pool;
                    tierColor = tier3Color; 
                    Tier3CostText.text = TierCosts[tier - 1].ToString(); 
                    break;
            }

            if (sourceList == null || sourceList.Count == 0)
            {
                Debug.LogWarning("No upgrades available for this tier!");
                return;
            }

            UpgradeData[] selectedOptions = new UpgradeData[3];

            if (tier == 3)
            {
                HeroStats heroStats = FindObjectOfType<HeroStats>();
                List<UpgradeData> validTier3Upgrades = new List<UpgradeData>();
                
                foreach (var upgrade in Tier3Upgrades)
                {
                    if (upgrade is StatUpgradeData statUpgrade)
                    {
                        if (statUpgrade.StatToUpgrade == StatType.WeaponSize && heroStats != null && heroStats.WeaponSizeLevel >= 2)
                        {
                            continue; // Maxed out, don't offer
                        }
                    }
                    validTier3Upgrades.Add(upgrade);
                }

                if (validTier3Upgrades.Count == 0)
                {
                    Debug.LogWarning("No valid Tier 3 upgrades available!");
                    return;
                }

                List<UpgradeData> tempSource = new List<UpgradeData>(validTier3Upgrades);
                for (int i = 0; i < 3; i++)
                {
                    if (tempSource.Count > 0)
                    {
                        int randomIndex = Random.Range(0, tempSource.Count);
                        selectedOptions[i] = tempSource[randomIndex];
                        tempSource.RemoveAt(randomIndex);
                    }
                    else
                    {
                        selectedOptions[i] = validTier3Upgrades[Random.Range(0, validTier3Upgrades.Count)];
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (currentPool.Count == 0)
                    {
                        RefillPool(currentPool, sourceList);
                    }

                    int lastIndex = currentPool.Count - 1;
                    selectedOptions[i] = currentPool[lastIndex];
                    currentPool.RemoveAt(lastIndex);
                }
            }

            if (rullete != null)
            {
                rullete.StartRullete(tierColor, selectedOptions);
                rulleteAnimator.SetTrigger("StartRullete");
                marketAnimator.SetTrigger("StartRullete");
            }
        }
    }

    private void RefillPool(List<UpgradeData> pool, List<UpgradeData> source)
    {
        pool.Clear();
        pool.AddRange(source);
        
        // Shuffle the pool using Fisher-Yates
        for (int i = 0; i < pool.Count; i++)
        {
            UpgradeData temp = pool[i];
            int randomIndex = Random.Range(i, pool.Count);
            pool[i] = pool[randomIndex];
            pool[randomIndex] = temp;
        }
    }
}
