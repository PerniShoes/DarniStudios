using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public int groupSize = 4;
    public int maxAlive = 60;
    public float minDistanceFromPlayer = 5f;
    public float maxDistanceFromPlayer = 20f;
    public float groupSpread = 3f;

    [Header("Map Walls")]
    public Transform leftWall;
    public Transform rightWall;
    public Transform topWall;
    public Transform bottomWall;

    private float timer;
    public static int aliveEnemies = 0;

    private void Start()
    {
        FindWalls();
    }
    private void Update()
    {
        if (!player || enemyPrefabs.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnGroup();
        }
    }

    private void SpawnGroup()
    {

        for (int i = 0; i < groupSize; i++)
        {
            if (aliveEnemies >= maxAlive) break;

            Vector3 spawnPos = GetRandomSpawnPosition();
            if (!IsInsideWalls(spawnPos)) continue;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = ObjectPoolManager.SpawnObject(prefab, spawnPos, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);
            
            // Shouldn't need to call GetComponent everytime
            // Code bellow is not good, just made to work for now
            //var ai = enemy.GetComponent<EnemyAI>();
            //if (ai != null)
            //{
            //    ai.enemyHP.currentHealth = ai.enemyHP.maxHealth;
            //    ai.enemyHP.healthBar.SetHealth(ai.enemyHP.currentHealth);
            //    ai.enemyHP.isDead = false;
            //    ai.player = player;
            //    ai.enabled = true;
            //}
            aliveEnemies++;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float distance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;
        Vector3 pos = player.position + offset;

        // Random group spread offset
        pos += new Vector3(Random.Range(-groupSpread, groupSpread), 0f, Random.Range(-groupSpread, groupSpread));

        return pos;
    }
    private bool IsInsideWalls(Vector3 pos)
    {
        if (!leftWall || !rightWall || !topWall || !bottomWall) return true;

        float leftX = leftWall.position.x;
        float rightX = rightWall.position.x;
        float topZ = topWall.position.z;
        float bottomZ = bottomWall.position.z;

        return (pos.x > leftX && pos.x < rightX && pos.z < topZ && pos.z > bottomZ);
    }
    private void FindWalls()
    {
        leftWall = GameObject.Find("LeftWall")?.transform ?? GameObject.FindGameObjectWithTag("WallLeft")?.transform;
        rightWall = GameObject.Find("RightWall")?.transform ?? GameObject.FindGameObjectWithTag("WallRight")?.transform;
        topWall = GameObject.Find("TopWall")?.transform ?? GameObject.FindGameObjectWithTag("WallTop")?.transform;
        bottomWall = GameObject.Find("BottomWall")?.transform ?? GameObject.FindGameObjectWithTag("WallBottom")?.transform;

        Debug.Log($"[Spawner] Walls detected:" +
                  $"\nLeft: {(leftWall ? leftWall.name : "❌")}" +
                  $"\nRight: {(rightWall ? rightWall.name : "❌")}" +
                  $"\nTop: {(topWall ? topWall.name : "❌")}" +
                  $"\nBottom: {(bottomWall ? bottomWall.name : "❌")}");
    }

}
