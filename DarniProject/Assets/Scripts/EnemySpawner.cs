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
    private static int aliveEnemies = 0;

    private void Start()
    {
        FindWalls(); // Always find walls at start
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
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

            var ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
                ai.player = player;

            aliveEnemies++;

            var destroyHandler = enemy.AddComponent<EnemyDestroyHandler>();
            destroyHandler.OnDestroyed += () => aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        }
    }

    // Generates a valid random spawn position around the player
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

    // Check if a point is within map walls
    private bool IsInsideWalls(Vector3 pos)
    {
        if (!leftWall || !rightWall || !topWall || !bottomWall) return true;

        float leftX = leftWall.position.x;
        float rightX = rightWall.position.x;
        float topZ = topWall.position.z;
        float bottomZ = bottomWall.position.z;

        return (pos.x > leftX && pos.x < rightX && pos.z < topZ && pos.z > bottomZ);
    }

    // Auto-finds walls each time the game starts
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

    public class EnemyDestroyHandler : MonoBehaviour
    {
        public System.Action OnDestroyed;
        private void OnDestroy() => OnDestroyed?.Invoke();
    }
}
