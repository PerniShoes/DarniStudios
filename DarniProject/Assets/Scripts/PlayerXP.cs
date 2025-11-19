using UnityEngine;
using UnityEngine.UI;
using TMPro; // ← dodaj to
using System.Collections.Generic;

public class PlayerXP : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;
    public float xpGrowthRate = 1.2f;

    [Header("Pickup Settings")]
    public float pickupRange = 3f;
    public float absorbDistance = 0.6f;
    public float pickupSpeed = 8f;
    public int expPerGem = 10;

    [Header("UI")]
    public Slider xpBar;
    public Text levelText;               // ← stary UI Text (zostaje)
    public TextMeshProUGUI levelTMP;     // ← nowy TMP text (opcjonalny)

    private List<Transform> activeGems = new List<Transform>();
    private float gemRefreshTimer = 0f;
    private const float gemRefreshInterval = 2f;

    private void Start()
    {
        UpdateUI();
        RefreshGemList();
    }

    private void Update()
    {
        gemRefreshTimer += Time.deltaTime;
        if (gemRefreshTimer >= gemRefreshInterval)
        {
            gemRefreshTimer = 0f;
            RefreshGemList();
        }

        AttractAndAbsorbGems();
    }

    private void RefreshGemList()
    {
        activeGems.Clear();
        GameObject[] gems = GameObject.FindGameObjectsWithTag("Gem");
        foreach (var gem in gems)
        {
            if (gem != null)
                activeGems.Add(gem.transform);
        }
    }

    private void AttractAndAbsorbGems()
    {
        if (activeGems.Count == 0) return;

        for (int i = activeGems.Count - 1; i >= 0; i--)
        {
            Transform gem = activeGems[i];
            if (gem == null)
            {
                activeGems.RemoveAt(i);
                continue;
            }

            Vector3 playerPos = transform.position + Vector3.up * 0.8f;
            float dist = Vector3.Distance(playerPos, gem.position);

            if (dist <= pickupRange)
            {
                float dynamicSpeed = Mathf.Lerp(pickupSpeed * 0.5f, pickupSpeed * 2f, 1f - (dist / pickupRange));
                gem.position = Vector3.MoveTowards(gem.position, playerPos, dynamicSpeed * Time.deltaTime);

                var anim = gem.GetComponent<Benjathemaker.SimpleGemsAnim>();
                if (anim != null)
                    anim.isBeingAttracted = true;
            }

            dist = Vector3.Distance(playerPos, gem.position);

            if (dist <= absorbDistance)
            {
                AddExp(expPerGem);
                GemManager.Instance.DespawnGem(gem.gameObject);
                activeGems.RemoveAt(i);
            }
        }
    }

    public void AddExp(int amount)
    {
        if (amount <= 0) return;

        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthRate);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (xpBar != null)
            xpBar.value = (float)currentXP / xpToNextLevel;

        if (levelText != null)
            levelText.text = $"LVL {currentLevel}";

        if (levelTMP != null)
            levelTMP.text = $"{currentLevel}"; // ← nowy TMP text
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 1f, 0.3f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
