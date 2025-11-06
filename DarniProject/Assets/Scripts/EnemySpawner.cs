using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform setplayer;          
    public GameObject[] enemyPrefabs; //enemy variants
    public float spawnEverySeconds = 1.5f;   
    public float radiusfromplayer = 15f;        

    float timer;

    void Update()
    {
        if (!setplayer || enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= spawnEverySeconds)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        // random point in circle from player
        Vector2 dir2D = Random.insideUnitCircle.normalized;
        Vector3 pos = setplayer.position + new Vector3(dir2D.x, 0f, dir2D.y) * radiusfromplayer;

        // random enemy variant
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var enemy = Instantiate(prefab, pos, Quaternion.identity);

        // show ai where player is // i know you hate that script but it works if you wanna you can change it
        var ai = enemy.GetComponentInChildren<EnemyAI>();
        if (!ai) ai = enemy.AddComponent<EnemyAI>();
        ai.player = setplayer;
    }
}
