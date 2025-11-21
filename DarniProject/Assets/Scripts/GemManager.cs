//using UnityEngine;
//using System.Collections.Generic;

//public class GemManager : MonoBehaviour
//{
//    public static GemManager Instance;

//    [Header("Pool Settings")]
//    public GameObject gemPrefab;
//    public int poolSize = 200;

//    private Queue<GameObject> gemPool = new Queue<GameObject>();

//    private void Awake()
//    {
//        Instance = this;
//        InitializePool();
//    }

//    private void InitializePool()
//    {
//        for (int i = 0; i < poolSize; i++)
//        {
//            GameObject gem = Instantiate(gemPrefab, transform);
//            gem.SetActive(false);
//            gemPool.Enqueue(gem);
//        }
//    }

//    public GameObject SpawnGem(Vector3 position)
//    {
//        if (gemPool.Count == 0)
//        {
//            GameObject extra = Instantiate(gemPrefab, transform);
//            extra.SetActive(false);
//            gemPool.Enqueue(extra);
//        }

//        GameObject gem = gemPool.Dequeue();
//        gem.transform.position = position;
//        gem.SetActive(true);
//        return gem;
//    }

//    public void DespawnGem(GameObject gem)
//    {
//        gem.SetActive(false);
//        gemPool.Enqueue(gem);
//    }
//}
