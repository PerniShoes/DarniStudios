using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(EnemyHP))]
public class EnemyDrop : MonoBehaviour
{
    [Header("EXP Drop Settings")]
    public GameObject expPrefab; 
    public int expAmount = 10;

    private EnemyHP enemyHP;

    void Start()
    {
        enemyHP = GetComponent<EnemyHP>();
    }

}
