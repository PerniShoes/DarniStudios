using UnityEngine;
using UnityEngine.UI;
using TMPro; // ← dodaj to
using System.Collections.Generic;

public class PlayerXP : MonoBehaviour
{
    [Header("XP Settings")]
    public int currentXP;
    public int currentLevel;
    public int xpToNextLevel;
    public float xpGrowthRate;

    [Header("Pickup Settings")]
    public float pickupRange;
    public float absorbDistance;
    public float pickupSpeed;
    public int expPerGem;

    [Header("UI")]
    public Slider xpBar;                 // >:C 
    public Text levelText;               // Stary UI Text 
    public TextMeshProUGUI levelTMP;     // Nowy TMP text 



    private Transform gemsFolder;
    void Awake()
    {
        gemsFolder = GameObject.Find("Gems").transform;
    }

    private void Start()
    {
        UpdateUI();

    }

    private void Update()
    {
        AttractAndAbsorbGems();
    }

    private void AttractAndAbsorbGems()
    {
        Vector3 playerPos = transform.position + Vector3.up * 0.8f;
        // This definitely can be better (loop over gems that are close, not all of them
        foreach (Transform gem in gemsFolder)
        {
            if (gem.gameObject.activeSelf == false) continue;
            float dist = Vector3.Distance(playerPos, gem.position);

            if (dist <= absorbDistance)
            {
                gem.position = Vector3.MoveTowards(gem.position, playerPos, pickupSpeed * Time.deltaTime);

                // GetComponent called too often
                var anim = gem.GetComponent<Benjathemaker.SimpleGemsAnim>();
                if (anim != null)
                    anim.isBeingAttracted = true;
            }

            dist = Vector3.Distance(playerPos, gem.position);

            if (dist <= pickupRange)
            {
                AddExp(expPerGem);
                ObjectPoolManager.ReturnObjectToPool(gem.gameObject);
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
