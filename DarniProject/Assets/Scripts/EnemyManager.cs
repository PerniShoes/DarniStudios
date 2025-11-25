using Benjathemaker;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;
    public int currentHealth;
    public bool isDead = false;
    public bool isBoss = false;

    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    public float attackRange = 2f;

    [Header("References")]
    public HealthBar healthBar;
    public Animator animator;
    public Transform player;

    [Header("EXP Drop Settings")]
    public GameObject expPrefab;
    public int expAmount = 10;

    private float destroyTime = 0.6f;
    private Coroutine destroyCoroutine;
    private KillCounter killCounter;

    void Awake()
    {
       
        if (healthBar == null)
            healthBar = GetComponentInChildren<HealthBar>();

        
        killCounter = Object.FindFirstObjectByType<KillCounter>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

 
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (isDead) return;

        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.magnitude > attackRange)
            {
                transform.position += direction.normalized * moveSpeed * Time.deltaTime;
                animator.SetBool("isMoveing", true);
                animator.SetBool("isAttack", false);
            }
            else
            {
                animator.SetBool("isMoveing", false);
                animator.SetBool("isAttack", true);
            }

            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }

        // Debug damage
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            TakeDamage(maxHealth / 2);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetBool("isDead", true);

        if (killCounter != null)
            killCounter.AddKill();

        if (isBoss && killCounter != null)
            killCounter.OnBossDefeated();

        destroyCoroutine = StartCoroutine(ReturnToPoolAfterTime());
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        EnemySpawner.aliveEnemies -= 1;

        if (expPrefab != null)
        {
            var gem = ObjectPoolManager.SpawnObject(expPrefab, transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Gems);
            var gemAnim = gem.GetComponent<SimpleGemsAnim>();
            if (gemAnim != null)
                gemAnim.DropGem();
        }

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
