using FullOpaqueVFX;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("LevelUpBonuses")]
    // Left to unlock
    public List<LevelUpBonus> allUnlockableSpells = new();
    public List<LevelUpBonus> allStatBonusUnlocks = new();
    // Already unlocked
    public Dictionary<LevelUpBonus, int> statBonusesLevels = new();
    public Dictionary<LevelUpBonus, int> spellSlots = new();


    [Header("StatAccess")]
    public PlayerTestHP healthStats;
    public Movement movementStats;


    void Awake()
    {
        //gemsFolder = GameObject.Find("Gems").transform;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }

    void Start()
    {
        gemsFolder = GameObject.Find("Gems").transform;
        LoadAllLevelUpBonuses();


        UpdateUI();
    }

    void Update()
    {
        AttractAndAbsorbGems();

        // LevelUp for testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddExp(xpToNextLevel);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            // Stops level ups
            xpToNextLevel = 100000;
        }
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

        // Level up
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;
            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthRate);

            PlayLevelUpSound();
            StateManager.Instance.PauseGame();
            ShowLevelUpChoices();
            // Game is Unpaused in LevelUpUIHandler in OnBonusSelected()

        }

        UpdateUI();
    }

    private void ShowLevelUpChoices()
    {
        LevelUpBonus[] choices = GetRandomChoices(3);
        LevelUpUI.Instance.ShowOptions(this, choices);

    }

    public void UnlockSpell(SpellData spellToUnlock)
    {

    }

    public LevelUpBonus[] GetRandomChoices(int count)
    {
        // If unlocking spell, remove it from the "Unlockable" list
        // If replacing a spell, can add the old one back to the list
        // If choosing a statBonus, check for maxLevel, if it's maxed, remove from list
        // For now for testing only adding stat bonuses


        // Coppies the whole list. Choose random index (store used ones) for better performance
        List<LevelUpBonus> pool = new List<LevelUpBonus>(allStatBonusUnlocks);
        LevelUpBonus[] choices = new LevelUpBonus[count];

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, pool.Count);
            choices[i] = pool[index];
            pool.RemoveAt(index); 
        }
        return choices;

    }

    private void LoadAllLevelUpBonuses()
    {
        allUnlockableSpells = Resources.LoadAll<LevelUpBonus>("LevelUpBonuses/SpellUnlocks").ToList();
        allStatBonusUnlocks = Resources.LoadAll<LevelUpBonus>("LevelUpBonuses/StatBonuses").ToList();
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
