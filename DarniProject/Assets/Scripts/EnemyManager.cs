
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public struct EnemyData
{
    public Vector3 position;
    public Quaternion rotation;
    public int maxHealth;
    public int currentHealth;
    public bool isDead;
    public bool isBoss;
    public float moveSpeed;
    public float attackRange;

    // public HealthBar healthBar;
    // public Animator animator;
    public float destroyDelay;
    public UnitStats statsReference; 

}
public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval;
    public int groupSize;
    public float minDistanceFromPlayer;
    public float maxDistanceFromPlayer;
    public float groupSpread;

    private Transform leftWall;
    private Transform rightWall;
    private Transform topWall;
    private Transform bottomWall;

    private float spawnIntervalTimer;

    //

    public Transform player;
    public KillCounter killCounter;
    public GameObject expPrefab;
    public static int aliveEnemies = 0;
    public GameObject[] enemyPrefabs;


    static public int maxAlive = 400;
    EnemyData[] enemies = new EnemyData[maxAlive];
    public List<GameObject> enemyViews = new List<GameObject>(); 

    void Start()
    {
        SetWalls();
    }
    void Update()
    {
        spawnIntervalTimer += Time.deltaTime;
        if (spawnIntervalTimer >= spawnInterval)
        {
            spawnIntervalTimer = 0f;
            SpawnGroup();
        }


        for (int i = 0; i < enemyViews.Count; i++)
        {
            if (!enemyViews[i].activeSelf) continue;

            UpdateEnemy(ref enemies[i], enemyViews[i]);
        }
    }

    private void SpawnGroup()
    {

        for (int i = 0; i < groupSize; i++)
        {
            if (aliveEnemies >= maxAlive) break;

            Vector3 spawnPos = GetRandomSpawnPosition();
            if (!IsInsideWalls(spawnPos)) continue; // To fix, it should find a position inside walls, not skip if it didn't

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            GameObject enemyView = ObjectPoolManager.SpawnObject(prefab, spawnPos, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);

            int slotIndex;
            if (enemyViews.Contains(enemyView)) 
            {
                slotIndex = enemyViews.IndexOf(enemyView);
            }
            else 
            {
                slotIndex = enemyViews.Count;
                enemyViews.Add(enemyView);
                enemies[slotIndex].statsReference = prefab.GetComponent<UnitStats>();
            }

            UnitStats stats = enemies[slotIndex].statsReference;

            enemies[slotIndex].position = spawnPos;
            enemies[slotIndex].rotation = Quaternion.identity;
            enemies[slotIndex].maxHealth = stats.maxHealth;
            enemies[slotIndex].currentHealth = stats.currentHealth;
            enemies[slotIndex].isDead = false;
            enemies[slotIndex].isBoss = stats.isBoss;
            enemies[slotIndex].moveSpeed = stats.moveSpeed;
            enemies[slotIndex].attackRange = stats.attackRange;
            enemies[slotIndex].destroyDelay = stats.destroyDelay;

            aliveEnemies++;
        }
    }


    private Vector3 GetRandomSpawnPosition()
    {
        float distance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;
        Vector3 pos = player.position + offset;

        // Random group spread offset
        pos += new Vector3(Random.Range(-groupSpread, groupSpread), 0f, Random.Range(-groupSpread, groupSpread));

        return pos;
    }


    void UpdateEnemy(ref EnemyData enemy, GameObject enemyView)
    {

        if (player != null)
        {
            Vector3 direction = player.position - enemy.position;
            direction.y = 0f;

            if (direction.magnitude > enemy.attackRange)
            {
                enemy.position += direction.normalized * enemy.moveSpeed * Time.deltaTime;
               //animator.SetBool("isMoveing", true);
               // animator.SetBool("isAttack", false);
            }
            else
            {
               // animator.SetBool("isMoveing", false);
               // animator.SetBool("isAttack", true);
            }

            if (direction != Vector3.zero)
            {
                enemy.rotation = Quaternion.LookRotation(direction);
            }
        }

    }



    private bool IsInsideWalls(Vector3 pos)
    {
        if (!leftWall || !rightWall || !topWall || !bottomWall) return true;

        float leftX = leftWall.position.x;
        float rightX = rightWall.position.x;
        float topZ = topWall.position.z;
        float bottomZ = bottomWall.position.z;

        return (pos.x > leftX && pos.x < rightX && pos.z < topZ && pos.z > bottomZ);
    }
    private void SetWalls()
    {
        leftWall = GameObject.Find("LeftWall")?.transform ?? GameObject.FindGameObjectWithTag("WallLeft")?.transform;
        rightWall = GameObject.Find("RightWall")?.transform ?? GameObject.FindGameObjectWithTag("WallRight")?.transform;
        topWall = GameObject.Find("TopWall")?.transform ?? GameObject.FindGameObjectWithTag("WallTop")?.transform;
        bottomWall = GameObject.Find("BottomWall")?.transform ?? GameObject.FindGameObjectWithTag("WallBottom")?.transform;

    }


}































//using Benjathemaker;
//using System.Collections;
//using UnityEngine;

//public class EnemyManager : MonoBehaviour
//{
//    [Header("Health Settings")]
//    public int maxHealth;
//    public int currentHealth;
//    public bool isDead = false;
//    public bool isBoss = false;

//    [Header("Movement Settings")]
//    public float moveSpeed = 3.5f;
//    public float attackRange = 2f;

//    [Header("References")]
//    public HealthBar healthBar;
//    public Animator animator;
//    public Transform player;

//    [Header("EXP Drop Settings")]
//    public GameObject expPrefab;
//    public int expAmount = 10;

//    private float destroyTime = 0.6f;
//    private Coroutine destroyCoroutine;
//    private KillCounter killCounter;

//    void Awake()
//    {

//        if (healthBar == null)
//            healthBar = GetComponentInChildren<HealthBar>();


//        killCounter = Object.FindFirstObjectByType<KillCounter>();

//        if (player == null)
//        {
//            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//            if (playerObj != null)
//                player = playerObj.transform;
//        }


//        if (animator == null)
//            animator = GetComponentInChildren<Animator>();
//    }

//    void Start()
//    {
//        currentHealth = maxHealth;

//        if (healthBar != null)
//            healthBar.SetMaxHealth(maxHealth);
//    }

//    void Update()
//    {
//        if (isDead) return;

//        if (player != null)
//        {
//            Vector3 direction = player.position - transform.position;
//            direction.y = 0f;

//            if (direction.magnitude > attackRange)
//            {
//                transform.position += direction.normalized * moveSpeed * Time.deltaTime;
//                animator.SetBool("isMoveing", true);
//                animator.SetBool("isAttack", false);
//            }
//            else
//            {
//                animator.SetBool("isMoveing", false);
//                animator.SetBool("isAttack", true);
//            }

//            if (direction != Vector3.zero)
//                transform.rotation = Quaternion.LookRotation(direction);
//        }

//        // Debug damage
//        if (Input.GetKeyDown(KeyCode.KeypadEnter))
//            TakeDamage(maxHealth / 2);
//    }

//    public void TakeDamage(int damage)
//    {
//        if (isDead) return;

//        currentHealth -= damage;
//        if (healthBar != null)
//            healthBar.SetHealth(currentHealth);

//        if (currentHealth <= 0)
//            Die();
//    }

//    void Die()
//    {
//        if (isDead) return;
//        isDead = true;

//        animator.SetBool("isDead", true);

//        if (killCounter != null)
//            killCounter.AddKill();

//        if (isBoss && killCounter != null)
//            killCounter.OnBossDefeated();

//        destroyCoroutine = StartCoroutine(ReturnToPoolAfterTime());
//    }

//    private IEnumerator ReturnToPoolAfterTime()
//    {
//        yield return new WaitForSeconds(destroyTime);
//        EnemySpawner.aliveEnemies -= 1;

//        if (expPrefab != null)
//        {
//            var gem = ObjectPoolManager.SpawnObject(expPrefab, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Gems);
//            var gemAnim = gem.GetComponent<SimpleGemsAnim>();
//            if (gemAnim != null)
//                gemAnim.DropGem();
//        }

//        ObjectPoolManager.ReturnObjectToPool(gameObject);
//    }
//}
