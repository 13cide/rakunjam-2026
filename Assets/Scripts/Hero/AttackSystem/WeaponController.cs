using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The transform that acts as the center of rotation (e.g. Hero center)")]
    public Transform WeaponPivot;
    [Tooltip("Array of hitboxes for different sizes. Index 0 = Small, 1 = Medium, 2 = Large")]
    public WeaponHitbox[] SizeHitboxes;
    
    public WeaponHitbox Hitbox { get; private set; }

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        if (WeaponPivot == null) WeaponPivot = transform;
        if (SizeHitboxes == null || SizeHitboxes.Length == 0)
        {
            WeaponHitbox childHitbox = GetComponentInChildren<WeaponHitbox>(true);
            if (childHitbox != null)
            {
                SizeHitboxes = new WeaponHitbox[] { childHitbox };
            }
        }
    }

    public void PerformAction(WeaponActionData action, HeroStats stats)
    {
        if (IsAttacking) return;
        SetActiveHitbox(stats.WeaponSizeLevel);
        StartCoroutine(ActionCoroutine(action, stats));
    }

    public void SetActiveHitbox(int level)
    {
        if (SizeHitboxes == null || SizeHitboxes.Length == 0) return;
        
        int index = Mathf.Clamp(level, 0, SizeHitboxes.Length - 1);
        for (int i = 0; i < SizeHitboxes.Length; i++)
        {
            if (SizeHitboxes[i] != null)
            {
                SizeHitboxes[i].gameObject.SetActive(i == index);
            }
        }
        Hitbox = SizeHitboxes[index];
    }

    private IEnumerator ActionCoroutine(WeaponActionData action, HeroStats stats)
    {
        IsAttacking = true;
        
        // Execute the custom logic inside the ScriptableObject
        yield return StartCoroutine(action.Execute(this, stats));
        
        IsAttacking = false;
    }
}
