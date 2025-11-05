using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public GameObject enemyPrefab;

    //  optional list of variant prefabs (different models/skins)
    [Tooltip("Optional: if set, a random variant will be used instead of 'enemyPrefab'.")]
    public GameObject[] enemyVariants; // leave empty to use 'enemyPrefab'

    public float spawnInterval = 1.5f;
    private float _timer;

    public float minSpawnDistance = 12f;
    public float maxSpawnDistance = 20f;
    [Range(30f, 170f)] public float behindConeAngle = 120f;

    public float groundProbeHeight = 30f;
    public LayerMask groundLayer = ~0;
    public float yOffset = 2f;

    public int maxAlive = 30;
    private int _aliveCount = 0;

    void Reset()
    {
        if (Camera.main != null) cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (player == null || cameraTransform == null) return;

        // If both enemyPrefab and enemyVariants are missing/empty, there's nothing to spawn
        bool hasBase = enemyPrefab != null;
        bool hasVariants = (enemyVariants != null && enemyVariants.Length > 0);
        if (!hasBase && !hasVariants) return;

        _timer += Time.deltaTime;
        if (_timer >= spawnInterval)
        {
            _timer = 0f;
            if (_aliveCount < maxAlive)
            {
                TrySpawnEnemy();
            }
        }
    }

    void TrySpawnEnemy()
    {
        Vector3 behindDir = -cameraTransform.forward;
        behindDir.y = 0f;
        if (behindDir.sqrMagnitude < 0.0001f) behindDir = -Vector3.forward;
        behindDir.Normalize();

        float half = behindConeAngle * 0.5f;
        float angle = Random.Range(-half, half);
        Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * behindDir;

        float dist = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector3 roughPos = player.position + dir * dist;

        Vector3 probeStart = new Vector3(roughPos.x, roughPos.y + groundProbeHeight, roughPos.z);
        Vector3 spawnPos = roughPos;

        if (Physics.Raycast(probeStart, Vector3.down, out RaycastHit hit, groundProbeHeight * 2f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            spawnPos = hit.point + Vector3.up * yOffset;
        }
        else
        {
            spawnPos.y = player.position.y + yOffset;
        }

        // pick a prefab (variant if available, otherwise fallback to enemyPrefab)
        GameObject prefabToSpawn = GetRandomPrefab();

        GameObject enemy = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        _aliveCount++;

        EnemyAI ai = enemy.GetComponent<EnemyAI>();
        if (ai == null) ai = enemy.AddComponent<EnemyAI>();
        ai.player = player;

        var tracker = enemy.AddComponent<SpawnedEnemyTracker>();
        tracker.onDestroyed = () => _aliveCount--;
    }

    // returns a random variant or the base prefab if no variants provided
    GameObject GetRandomPrefab()
    {
        // If we have at least one variant, pick a random one
        if (enemyVariants != null && enemyVariants.Length > 0)
        {
            int i = Random.Range(0, enemyVariants.Length);
            if (enemyVariants[i] != null) return enemyVariants[i];

            // If the chosen slot is null, try to find any non-null variant
            for (int k = 0; k < enemyVariants.Length; k++)
                if (enemyVariants[k] != null) return enemyVariants[k];
        }

        // Fallback to base prefab
        return enemyPrefab;
    }

    void OnDrawGizmosSelected()
    {
        if (player == null || cameraTransform == null) return;

        Gizmos.color = new Color(0f, 0.6f, 1f, 0.35f);

        Vector3 origin = player.position;
        Vector3 behindDir = -cameraTransform.forward;
        behindDir.y = 0f; behindDir.Normalize();

        int steps = 30;
        float half = behindConeAngle * 0.5f;

        for (int i = 0; i < steps; i++)
        {
            float t0 = (i / (float)steps) * behindConeAngle - half;
            float t1 = ((i + 1) / (float)steps) * behindConeAngle - half;

            Vector3 d0 = Quaternion.AngleAxis(t0, Vector3.up) * behindDir;
            Vector3 d1 = Quaternion.AngleAxis(t1, Vector3.up) * behindDir;

            Vector3 p0a = origin + d0 * minSpawnDistance;
            Vector3 p1a = origin + d1 * minSpawnDistance;
            Vector3 p0b = origin + d0 * maxSpawnDistance;
            Vector3 p1b = origin + d1 * maxSpawnDistance;

            Gizmos.DrawLine(p0a, p1a);
            Gizmos.DrawLine(p0b, p1b);
            if (i % 5 == 0)
            {
                Gizmos.DrawLine(p0a, p0b);
            }
        }
    }

    private class SpawnedEnemyTracker : MonoBehaviour
    {
        public System.Action onDestroyed;
        void OnDestroy() { onDestroyed?.Invoke(); }
    }
}
