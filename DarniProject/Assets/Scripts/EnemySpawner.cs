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

            // Twój kod generował losową pozycje po czym tutaj sprawdzał czy jest w min dystansie. Jeśli nie jest to "continue" czyli pomijał dany resp
            // Czyli ten resp zamiast pójść gdzie indziej to wgl się nie dział. Jak min dystans był za duży to wtedy naturalnie nic, nigdy się nie respiło

            // Na przyszłość: Zamiast generować losowo a potem sprawdzać czy jest okej i musieć generować znowu, to lepiej od razu wygenerować losowy, 
            // ale poprawny punkt. Teraz w GetRandomSpawnPosition respi losowy punkt, ale w odpowiednim zakresie

            //if (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer)
            //    continue;

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
        float x, z;

        // Check Random.value return    (it's just a 50/50 here)
        if (Random.value < 0.5f)
        {
            x = Random.Range(-maxDistanceFromPlayer, maxDistanceFromPlayer);
            z = Random.value < 0.5f
                ? Random.Range(-maxDistanceFromPlayer, -minDistanceFromPlayer)
                : Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        }
        else
        {
            z = Random.Range(-maxDistanceFromPlayer, maxDistanceFromPlayer);
            x = Random.value < 0.5f
                ? Random.Range(-maxDistanceFromPlayer, -minDistanceFromPlayer)
                : Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        }

        return player.position + new Vector3(x, 0f, z);
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
