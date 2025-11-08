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
    public float spawnRadius = 40f;
    public float minDistanceFromPlayer = 10f;
    public float groupSpread = 3f;

    [Header("Map Bounds")]
    public Transform leftWall;
    public Transform rightWall;
    public Transform topWall;
    public Transform bottomWall;

    private float timer;
    private static int aliveEnemies = 0;

    void Start()
    {
        AutoFindWalls(); // Auto wall finding
    }

    void Update()
    {
        if (!player || enemyPrefabs.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnGroup();
        }
    }

    void SpawnGroup()
    {
        for (int i = 0; i < groupSize; i++)
        {
            if (aliveEnemies >= maxAlive) break;

            Vector3 spawnPos = GetRandomSpawnPosition();

            if (!IsInsideWalls(spawnPos))
                continue;

            if (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer)
                continue;

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            var ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
                ai.player = player;

            aliveEnemies++;

            var destroyHandler = enemy.AddComponent<EnemyDestroyHandler>();
            destroyHandler.OnDestroyed += () => aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 r = Random.insideUnitCircle * spawnRadius;
        Vector3 pos = player.position + new Vector3(r.x, 0f, r.y);
        pos += new Vector3(Random.Range(-groupSpread, groupSpread), 0f, Random.Range(-groupSpread, groupSpread));
        return pos;
    }

    bool IsInsideWalls(Vector3 pos)
    {
        if (!leftWall || !rightWall || !topWall || !bottomWall)
            return true; // If walls not found dont block spawn

        float leftX = leftWall.position.x;
        float rightX = rightWall.position.x;
        float topZ = topWall.position.z;
        float bottomZ = bottomWall.position.z;

        bool insideX = pos.x > leftX && pos.x < rightX;
        bool insideZ = pos.z < topZ && pos.z > bottomZ;

        return insideX && insideZ;
    }

    void AutoFindWalls()
    {
        // Finding walls from Names
        if (!leftWall)
            leftWall = GameObject.Find("LeftWall")?.transform;
        if (!rightWall)
            rightWall = GameObject.Find("RightWall")?.transform;
        if (!topWall)
            topWall = GameObject.Find("TopWall")?.transform;
        if (!bottomWall)
            bottomWall = GameObject.Find("BottomWall")?.transform;

        // IDK if its good but if not fin names check wall tags
        if (!leftWall)
            leftWall = GameObject.FindGameObjectWithTag("WallLeft")?.transform;
        if (!rightWall)
            rightWall = GameObject.FindGameObjectWithTag("WallRight")?.transform;
        if (!topWall)
            topWall = GameObject.FindGameObjectWithTag("WallTop")?.transform;
        if (!bottomWall)
            bottomWall = GameObject.FindGameObjectWithTag("WallBottom")?.transform;

        // Log to know what to find
        Debug.Log($"[Spawner] Ściany znalezione: " +
                  $"\nLewa: {(leftWall ? leftWall.name : "❌")}" +
                  $"\nPrawa: {(rightWall ? rightWall.name : "❌")}" +
                  $"\nGóra: {(topWall ? topWall.name : "❌")}" +
                  $"\nDół: {(bottomWall ? bottomWall.name : "❌")}");
    }

    public class EnemyDestroyHandler : MonoBehaviour
    {
        public System.Action OnDestroyed;
        void OnDestroy() => OnDestroyed?.Invoke();
    }
}
