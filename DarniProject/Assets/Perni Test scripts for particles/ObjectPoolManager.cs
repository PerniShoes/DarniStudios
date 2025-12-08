using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject _objectPoolHolder;

    private static GameObject _particleSystemHolder;
    private static GameObject _enemiesHolder;
    private static GameObject _gemsHolder;
    private static GameObject _UIHolder;
    private static GameObject _otherHolder;

    public enum PoolType
    {
        ParticleSystem,
        Enemies,
        Gems,
        Other,
        UI,
        None
    }

    private static PoolType PoolingType;
    private void Awake()
    {
        SetupFolderStructure();
    }
    private void SetupFolderStructure()
    {
        _objectPoolHolder = new GameObject("Pooled Objects");

        _particleSystemHolder = new GameObject("Particle Effects");
        _particleSystemHolder.transform.SetParent(_objectPoolHolder.transform);

        _enemiesHolder = new GameObject("Enemies");
        _enemiesHolder.transform.SetParent(_objectPoolHolder.transform);

        _gemsHolder = new GameObject("Gems");
        _gemsHolder.transform.SetParent(_objectPoolHolder.transform);

        _UIHolder = new GameObject("UI");
        _UIHolder.transform.SetParent(_objectPoolHolder.transform);

        _otherHolder = new GameObject("Other");
        _otherHolder.transform.SetParent(_objectPoolHolder.transform);

    }

    public static bool HasInactive(GameObject prefab)
    {
        // This function stems from having single ObjectPools manage limitted amount of objects with multiple prefabs 
        // EnemyManager - EM, ObjectPoolManager - OPM

        // E.g.: EM wants max 3 alive enemies. It has: X, Y, Z. Active: X Inactive: Y, Z
        // EM knows how many objects are active/alive, but doesn't know which ones. It sees 1 alive object (correct)
        // EM calls OPM to spawn an enemy, randomly choosing one from X, Y, Z. It lands on X
        // OPM tries to spawn an X enemy, but sees no inactive objects to use. So it creates a second X enemy
        // Now there are 4 objects in the pool: X, X, Y, Z (instead of 3 max)

        PooledObjectInfo pool = ObjectPools.Find(storedPools => storedPools.LookupString == prefab.name);
        if(pool == null)
        {
            return true;
        }
        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();
        if (spawnableObject == null)
        {
            return false;
        }
        return true;
    }

    public static GameObject SpawnObject(GameObject target, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(storedPools => storedPools.LookupString == target.name);

        if(pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = target.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObject == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObject = Instantiate(target, position, rotation);


            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObject.transform.position = position;
            spawnableObject.transform.rotation = rotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    // Overload for objects that have parent's transform
    public static GameObject SpawnObject(GameObject target,Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(storedPools => storedPools.LookupString == target.name);

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = target.name };
            ObjectPools.Add(pool);
        }
        GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(target, parentTransform);
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static void ReturnObjectToPool(GameObject target)
    {
        // Cuts out last 7 chars -> "(Clone)" from name
        string nameWithoutClone = target.name.Substring(0, target.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(storedPools => storedPools.LookupString == nameWithoutClone);

        if(pool == null)
        {
            Debug.LogWarning("Trying to return an object without a pool: " + target.name);
        }
        else
        {
            target.SetActive(false);
            pool.InactiveObjects.Add(target);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemHolder;
            case PoolType.Enemies:
                return _enemiesHolder;
            case PoolType.Gems:
                return _gemsHolder;
            case PoolType.UI:
                return _UIHolder;
            case PoolType.Other:
                return _otherHolder;
            case PoolType.None:
                return null;

            default:
                return null;
        }
    }


}


public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();

}