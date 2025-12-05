using UnityEngine;
using System.Collections.Generic;

public class EnemyScriptAttacher : MonoBehaviour 
{

    [Header("Script Attachment Settings")]
    public bool applyOnStart = true;
    public float checkInterval = 2f;

    [Header("Settings")]
    public string enemyTag = "Enemy";
    public bool debugLog = true;

    private void Start()
    {
        if (applyOnStart)
            StartCoroutine(LoopEnemies());
    }

    System.Collections.IEnumerator LoopEnemies()
    {
        while(true)
        {
            ApplyToAllEnemies();
            yield return new WaitForSeconds(checkInterval);
        }
    }
    void ApplyToAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy) continue;
           
            if (enemy.GetComponent<EnemyManager>() == null)
            {
                enemy.AddComponent<EnemyManager>();
            }
        }
    }

}
