using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerXP : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;
    public float xpGrowthRate = 1.2f;

    [Header("Pickup Settings")]
    public float pickupRange = 2.5f;
    public float pickupSpeed = 8f;
    public int expPerGem = 10;

    [Header("UI (Work in Progress)")]
    public Slider xpBar;
    public Text levelText;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        AttractNearbyGems();
    }

    // Adds XP to player
    public void AddExp(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;

        // Level Up logic
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthRate);
        }

        UpdateUI();
    }

    // Pulls nearby XP gems toward player
    private void AttractNearbyGems()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange, ~0, QueryTriggerInteraction.Collide);

        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Gem")) continue;

            Transform gem = hit.transform;
            Vector3 targetPos = transform.position + Vector3.up * 1f;

            // Stop floating anim
            var anim = gem.GetComponent<Benjathemaker.SimpleGemsAnim>();
            if (anim != null) anim.isBeingAttracted = true;

            // Move toward player
            gem.position = Vector3.MoveTowards(gem.position, targetPos, pickupSpeed * Time.deltaTime);

            // Absorb
            float distance = Vector3.Distance(gem.position, transform.position);
            if (distance <= 2f) // enlarged pickup radius
            {
                AddExp(expPerGem);
                Destroy(gem.gameObject);
            }
        }
    }

    // Update XP bar and level text
    private void UpdateUI()
    {
        if (xpBar != null)
            xpBar.value = (float)currentXP / xpToNextLevel;

        if (levelText != null)
            levelText.text = $"LVL {currentLevel}";
    }

    // Draw pickup range for debug in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 1f, 0.3f, 0.25f);
        Gizmos.DrawSphere(transform.position, pickupRange);
    }
}
