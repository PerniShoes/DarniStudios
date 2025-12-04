using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public AttackTypes attackType;
    public float damageTriggerNormalizedTime;
    public bool isRanged;
    public int damage;
    public Vector3 targetOffset;

    public float destroyDelay;
}
public class EnemyViewData
{
    public GameObject gameObjectRef;
    public Animator animator;
    public Rigidbody rb;
    public UnitStats statsReference;
    public HealthBar healthbarReference;
    public float lastAttackAnimStep;

}

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval;
    public int groupSize;
    public float minDistanceFromPlayer;
    public float maxDistanceFromPlayer;
    public float groupSpread;
    public float targetRadiusAroundPlayer;

    private Transform leftWall;
    private Transform rightWall;
    private Transform topWall;
    private Transform bottomWall;

    private float spawnIntervalTimer;

    public Transform player;
    public PlayerTestHP playerHPScript;
    public KillCounter killCounter;
    public GameObject expPrefab;
    public static int aliveEnemies = 0;
    public GameObject[] enemyPrefabs;

    static public int maxAlive = 10;
    EnemyData[] enemies = new EnemyData[maxAlive];
    EnemyViewData[] enemyViewData = new EnemyViewData[maxAlive];
    private int availableSlot = 0;

    private int updateIndex = 0;
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

    }

    private void FixedUpdate()
    {
        int enemiesPerFrame = enemyViewData.Length / 1;
        for (int i = 0; i < enemiesPerFrame; i++)
        {
            if (enemyViewData[updateIndex] == null || !enemyViewData[updateIndex].gameObjectRef.activeSelf)
            {
                updateIndex = (updateIndex + 1) % enemyViewData.Length;
                continue;
            }

            UpdateEnemy(ref enemies[updateIndex], enemyViewData[updateIndex]);
            updateIndex = (updateIndex + 1) % enemyViewData.Length;
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

            if (availableSlot >= maxAlive && !ObjectPoolManager.HasInactive(prefab))
            {
                return; // Skips spawning, to fix
            }
            GameObject enemyView = ObjectPoolManager.SpawnObject(prefab, spawnPos, Quaternion.identity, ObjectPoolManager.PoolType.Enemies);

            int slotIndex = -1;
            for (int index = 0; index < enemyViewData.Length; index++)
            {
                if (enemyViewData[index] != null && enemyViewData[index].gameObjectRef == enemyView)
                {
                    slotIndex = index;
                    break;
                }
            }

            // Newly Instantiated Object
            if (slotIndex == -1)
            {
                slotIndex = availableSlot;

                enemyViewData[slotIndex] = new EnemyViewData();
                enemyViewData[slotIndex].gameObjectRef = enemyView;
                enemyViewData[slotIndex].rb = enemyView.GetComponent<Rigidbody>();
                enemyViewData[slotIndex].animator = enemyView.GetComponent<Animator>();
                enemyViewData[slotIndex].statsReference = enemyView.GetComponent<UnitStats>();
                enemyViewData[slotIndex].healthbarReference = enemyView.GetComponentInChildren<HealthBar>();

                availableSlot++;
            }

            UnitStats stats = enemyViewData[slotIndex].statsReference;

            Vector2 randomCircle = Random.insideUnitCircle * targetRadiusAroundPlayer;
            enemies[slotIndex].targetOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
            enemies[slotIndex].position = spawnPos;
            enemies[slotIndex].rotation = Quaternion.identity;
            enemies[slotIndex].maxHealth = stats.maxHealth;
            enemies[slotIndex].currentHealth = stats.currentHealth;
            enemies[slotIndex].isDead = false;
            enemies[slotIndex].isBoss = stats.isBoss;
            enemies[slotIndex].moveSpeed = stats.moveSpeed;
            enemies[slotIndex].attackRange = stats.attackRange;
            enemies[slotIndex].isRanged = stats.isRanged;
            enemies[slotIndex].attackType = stats.attackType;
            enemies[slotIndex].damageTriggerNormalizedTime = stats.damageTriggerNormalizedTime;


            enemies[slotIndex].destroyDelay = stats.destroyDelay;
            enemies[slotIndex].damage = stats.damage;
            enemyViewData[slotIndex].healthbarReference.SetHealth(100); // This works with %... Have to rewrite Healthbar
            aliveEnemies++;
        }
    }
    void UpdateEnemy(ref EnemyData enemy, EnemyViewData enemyView)
    {
        if (player == null) return;
   
        Vector3 targetPosition = player.position + enemy.targetOffset;
        Vector3 toTarget = targetPosition - enemyView.rb.position;
        toTarget.y = 0f;
        float distanceToTarget = toTarget.magnitude;

        Vector3 toPlayer = player.position - enemyView.rb.position;
        toPlayer.y = 0f;

        Vector3 moveDirection;
        if (toPlayer.magnitude > targetRadiusAroundPlayer)
        {
            moveDirection = toTarget.normalized;
        }
        else
        {
            moveDirection = (player.position - enemyView.rb.position).normalized;
        }

        Animator animator = enemyView.animator;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Death")) 
        { 
            return; 
        }


        ////////// Attack damage dealing logic
        if (!stateInfo.IsName("attack"))
        {
            enemyView.lastAttackAnimStep = -1;
        }
        else
        {
            float t = stateInfo.normalizedTime;
            float interval = 1f;
            float offset = enemy.damageTriggerNormalizedTime;
            int currentStep = Mathf.FloorToInt((t - offset) / interval);

            if (currentStep != enemyView.lastAttackAnimStep && t >= offset)
            {
                enemyView.lastAttackAnimStep = currentStep;

                float distanceTolerance = 0.5f;
                // Placeholder checking. Have to account for things like units moving, shape of attack, AOE, etc.
                if (toPlayer.sqrMagnitude < (enemy.attackRange * enemy.attackRange) + distanceTolerance)
                {
                    DamagePlayer(enemy.damage);
                }
            }
        }
        ////////// 
        ///
        if (toPlayer.magnitude > enemy.attackRange)
        {
            if (animator != null)
            {
                animator.SetBool("isAttack", false);
                if (!stateInfo.IsName("attack"))
                {
                    animator.SetBool("isMoveing", true);
                    enemyView.rb.MovePosition(enemyView.rb.position + moveDirection * enemy.moveSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isMoveing", false);
                animator.SetBool("isAttack", true);
            }
        }

        if (moveDirection != Vector3.zero)
        {
            if (!stateInfo.IsName("attack"))
            {
                enemy.rotation = Quaternion.LookRotation(moveDirection);
                enemyView.gameObjectRef.transform.rotation = enemy.rotation;
            }
        }
    }

    public void DamagePlayer(int damage)
    {
        playerHPScript.TakeDamage(damage);
    }
    public void DamageEnemy(ref GameObject enemy, int damage)
    {
        for (int i = 0; i < enemyViewData.Length; i++)
        {
            if (enemyViewData[i] != null && enemyViewData[i].gameObjectRef == enemy)
            {
                enemies[i].currentHealth -= damage;
                enemyViewData[i].healthbarReference.SetHealth(enemies[i].currentHealth);

                if (enemies[i].currentHealth <= 0 && !enemies[i].isDead)
                {
                    enemies[i].isDead = true;

                    enemyViewData[i].animator.SetBool("isDead", true);

                    killCounter.AddKill();
                    if (expPrefab != null)
                    {
                        ObjectPoolManager.SpawnObject(
                            expPrefab,
                            enemy.transform.position,
                            Quaternion.identity,
                            ObjectPoolManager.PoolType.Gems
                        );
                    }

                    StartCoroutine(ReturnEnemyToPool(i));
                }

                return; 
            }
        }
    }
    private IEnumerator ReturnEnemyToPool(int index)
    {
        yield return new WaitForSeconds(enemies[index].destroyDelay);

        ObjectPoolManager.ReturnObjectToPool(enemyViewData[index].gameObjectRef);
        --aliveEnemies;
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

