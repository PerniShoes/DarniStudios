using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(EnemyHP))]
public class EnemyDrop : MonoBehaviour
{
    [Header("EXP Drop Settings")]
    public GameObject expPrefab; // XP prefab
    public int expAmount = 10;

    private EnemyHP enemyHP;

    void Start()
    {
        // Try to assign prefab automatically if not already set
        if (expPrefab == null)
        {
#if UNITY_EDITOR
            // Load prefab directly from project path (Editor only)
            string prefabPath = "Assets/Assets/EnemyDrops/BTM_Items_Gems/Prefabs/XpGem.prefab";
            expPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (expPrefab == null)
                Debug.LogWarning($"⚠️ EnemyDrop on {gameObject.name}: Could not find prefab at {prefabPath}");
#else
            Debug.LogWarning($"⚠️ EnemyDrop on {gameObject.name}: expPrefab not assigned. Make sure it's set before building!");
#endif
        }

        enemyHP = GetComponent<EnemyHP>();
        // enemyHP.OnDeath += DropExp;
    }

    void DropExp(EnemyHP enemy)
    {
        if (expPrefab == null) return;

        Vector3 offset = new Vector3(Random.Range(-0.4f, 0.4f), 0.6f, Random.Range(-0.4f, 0.4f));
        Instantiate(expPrefab, transform.position + offset, Quaternion.identity);
    }
}
