using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance { get; private set; }

    [Header("UI Settings")]
    public TMP_Text counterText;
    public int killGoal = 500;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public Transform player;        
    public float spawnDistance = 5f; 

    private int currentKills = 0;
    private bool bossSpawned = false;

    void Start()
    {
        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        UpdateCounterUI();
    }

    public void AddKill()
    {
        if (bossSpawned) return;

        currentKills++;
        UpdateCounterUI();

        if (currentKills >= killGoal)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossSpawned || bossPrefab == null || player == null) return;

        bossSpawned = true;
        UpdateCounterUI();

        
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0; 
        randomDirection.Normalize();

        Vector3 spawnPosition = player.position + randomDirection * spawnDistance;

        ObjectPoolManager.SpawnObject(bossPrefab, spawnPosition, Quaternion.identity,ObjectPoolManager.PoolType.Other);
       
    }

    public void OnBossDefeated()
    {
        currentKills = 0;
        killGoal += 50;
        bossSpawned = false;
        UpdateCounterUI();
    }

    void UpdateCounterUI()
    {
        if (counterText != null)
            counterText.text = $"{currentKills}/{killGoal}";
    }
}
