using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rullete : MonoBehaviour
{
    [SerializeField] private Image[] optionsBackGround;
    [Tooltip("Parallel to optionsBackGround / optionsText. Each slot is the icon Image on that option card.")]
    [SerializeField] private Image[] optionsIcon;
    [SerializeField] private TMP_Text[] optionsText;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private Animator rulleteAnimator;
    [SerializeField] private Animator marketAnimator;

    private UpgradeData[] currentOptions;
    
    public void StartRullete(Color backgroundcolor, UpgradeData[] options)
    {
        currentOptions = options;

        foreach (var n in optionsBackGround) {
            n.color = backgroundcolor;
        }
        for (int i = 0; i < options.Length; i++) {
            if (options[i] == null) continue;

            if (optionsText[i] != null)
            {
                optionsText[i].text = options[i].Description;
            }

            if (optionsIcon != null && i < optionsIcon.Length && optionsIcon[i] != null)
            {
                optionsIcon[i].sprite = options[i].Icon;
                optionsIcon[i].enabled = options[i].Icon != null;   // hides the slot if an SO has no icon assigned
            }
        }
    }
    
    public void SelectOption(int index)
    {
        if (currentOptions == null || index < 0 || index >= currentOptions.Length) return;

        UpgradeData selectedUpgrade = currentOptions[index];
        
        if (selectedUpgrade != null && upgradeManager != null)
        {
            upgradeManager.AddUpgrade(selectedUpgrade);
            rulleteAnimator.SetTrigger("StopRullete");
            marketAnimator.SetTrigger("StopRullete");
        }
    }
}