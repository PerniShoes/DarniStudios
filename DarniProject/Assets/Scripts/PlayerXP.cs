using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Slider xpBar;
    public TextMeshProUGUI levelTMP;

    [Header("Audio")]
    public AudioClip levelUpSound;
    public float levelUpVolume;

    private AudioSource audioSource;
    private Transform gemsFolder;

    void Awake()
    {
        //gemsFolder = GameObject.Find("Gems").transform;

        // Add or find AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Start()
    {
        gemsFolder = GameObject.Find("Gems").transform;
        UpdateUI();
    }

    void Update()
    {
        AttractAndAbsorbGems();
    }

    private void AttractAndAbsorbGems()
    {
        Vector3 playerPos = transform.position + Vector3.up * 0.8f;

        foreach (Transform gem in gemsFolder)
        {
            // This loop is checking every active gem everytime. Even if for example 90% of them are far far away
            if (!gem.gameObject.activeSelf) continue;

            float dist = Vector3.Distance(playerPos, gem.position);

            if (dist <= absorbDistance)
            {
                gem.position = Vector3.MoveTowards(gem.position, playerPos, pickupSpeed * Time.deltaTime);
                // Get component shouldn't be called everytime here. It should be stored/cached somewhere per gem
                var anim = gem.GetComponent<Benjathemaker.SimpleGemsAnim>();
                if (anim != null) anim.isBeingAttracted = true;
            }

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
        bool leveledUp = false;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthRate);
            leveledUp = true;
        }

        if (leveledUp)
            PlayLevelUpSound();

        UpdateUI();
    }

    private void PlayLevelUpSound()
    {
        if (levelUpSound != null && audioSource != null)
            audioSource.PlayOneShot(levelUpSound, levelUpVolume);
    }

    private void UpdateUI()
    {
        if (xpBar != null)
            xpBar.value = (float)currentXP / xpToNextLevel;

        if (levelTMP != null)
            levelTMP.text = $"{currentLevel}";
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.3f, 1f, 0.3f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
