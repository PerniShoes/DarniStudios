using UnityEngine;

public class EnemyAutoSetup : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Tag of the enemies to automatically configure.")]
    public string enemyTag = "Enemy";

    [Tooltip("Enable to show debug logs in the console.")]
    public bool showDebugLogs = true;

    void Start()
    {
        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            AddMissingScripts(enemy);
        }

        if (showDebugLogs)
            Debug.Log($"✅ EnemyAutoSetup: Updated {enemies.Length} enemies in the scene.");
    }

    private void AddMissingScripts(GameObject enemy)
    {
        // EnemyHP
        if (enemy.GetComponent<EnemyHP>() == null)
        {
            enemy.AddComponent<EnemyHP>();
            if (showDebugLogs) Debug.Log($"Added EnemyHP to {enemy.name}");
        }

        // EnemyAI
        if (enemy.GetComponent<EnemyAI>() == null)
        {
            enemy.AddComponent<EnemyAI>();
            if (showDebugLogs) Debug.Log($"Added EnemyAI to {enemy.name}");
        }

        // EnemyDrop
        if (enemy.GetComponent<EnemyDrop>() == null)
        {
            enemy.AddComponent<EnemyDrop>();
            if (showDebugLogs) Debug.Log($"Added EnemyDrop to {enemy.name}");
        }
    }
}
