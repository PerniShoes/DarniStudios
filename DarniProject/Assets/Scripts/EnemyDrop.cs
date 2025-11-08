using UnityEngine;

[RequireComponent(typeof(EnemyHP))]
public class EnemyDrop : MonoBehaviour
{
    [Header("EXP Drop Settings")]
    public GameObject expPrefab; // prefab XP
    public int expAmount = 10;   

    private EnemyHP enemyHP;

    void Start()
    {
        enemyHP = GetComponent<EnemyHP>();
        enemyHP.OnDeath += DropExp; 
    }

    void DropExp(EnemyHP enemy)
    {
        if (expPrefab == null) return;

        int gemCount = Mathf.Max(1, expAmount / 10); 
        for (int i = 0; i < gemCount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.4f, 0.4f), 0.6f, Random.Range(-0.4f, 0.4f));
            GameObject gem = Instantiate(expPrefab, transform.position + offset, Quaternion.identity);

            
            Rigidbody rb = gem.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
    }
}

