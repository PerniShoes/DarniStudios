using Benjathemaker;
using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;
    public int currentHealth;

    [Header("References")]
    public HealthBar healthBar;
    public Animator animator;

    public bool isDead = false;

    [Header("EXP Drop Settings")]
    public GameObject expPrefab;
    public int expAmount = 10;

    private float destroyTime = 0.6f;
    // Not sure why I store the Coroutine instead of just calling it, tbh
    Coroutine _returnToPoolTimerCoroutine;

    private EnemyAI ai;

    void Awake()
    {
        // This seems like it shouldn't be needed (it is rn)
        if (healthBar == null)
            healthBar = GetComponentInChildren<HealthBar>();
    }
    void Start()
    {
        maxHealth = 300;
        currentHealth = maxHealth;

        ai = GetComponent<EnemyAI>();

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }


    void Update()
    {
        // Debug damage
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TakeDamage(maxHealth/2);
        }
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

        if (ai != null) ai.enabled = false;

        if (animator != null)
            animator.SetBool("isDead", true);

        _returnToPoolTimerCoroutine = StartCoroutine(ReturnToPoolAfterTime());
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        float elapsedTime = 0f;
        while(elapsedTime < destroyTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        EnemySpawner.aliveEnemies -= 1;

        var gem = ObjectPoolManager.SpawnObject(expPrefab, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Gems);
        // This could probably be better
        var gemAnim = gem.GetComponent<SimpleGemsAnim>();
        gemAnim.DropGem();

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

}
