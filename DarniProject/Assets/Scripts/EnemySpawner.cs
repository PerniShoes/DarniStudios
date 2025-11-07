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

    [Header("Detection")]
    public LayerMask groundMask;
    public LayerMask obstacleMask;

    private float timer;
    private static int aliveEnemies = 0;

    void Update()
    {
        if (!player || enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

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

            Vector3 spawnPos;
            if (!TryGetValidSpawn(out spawnPos)) continue;

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

    bool TryGetValidSpawn(out Vector3 spawnPos)
    {
        for (int attempt = 0; attempt < 15; attempt++)
        {
            Vector2 r = Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = player.position + new Vector3(r.x, 10f, r.y);

            // Searching for ground/some sort of flor
            if (Physics.Raycast(candidate, Vector3.down, out RaycastHit hit, 20f, groundMask))
            {
                Vector3 pos = hit.point;

                // Not to close to player
                if (Vector3.Distance(pos, player.position) < minDistanceFromPlayer)
                    continue;

                // Avoid spawning in walls
                if (Physics.CheckSphere(pos, 1f, obstacleMask))
                    continue;

                // OP code cheking if player see walls and avoid to spawn enemy behind them
                Vector3 dirToPlayer = (player.position - pos).normalized;
                float distToPlayer = Vector3.Distance(pos, player.position);

                if (Physics.Raycast(pos + Vector3.up * 1f, dirToPlayer, distToPlayer, obstacleMask))
                {
                    // Id REYCAST hit wall cancel spawning
                    continue;
                }

                spawnPos = pos + Random.insideUnitSphere * groupSpread;
                spawnPos.y = hit.point.y;
                return true;
            }
        }

        spawnPos = Vector3.zero;
        return false;
    }


    public class EnemyDestroyHandler : MonoBehaviour
    {
        public System.Action OnDestroyed;
        void OnDestroy() => OnDestroyed?.Invoke();
    }
}
