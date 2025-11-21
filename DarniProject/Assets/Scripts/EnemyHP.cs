using System.Collections;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth;
    private int currentHealth;

    [Header("References")]
    public HealthBar healthBar;
    public Animator animator;

    private bool isDead = false;

    public delegate void EnemyDeathEvent(EnemyHP enemy);
    private float destroyTime = 1.5f;
    // Not sure why I store the Coroutine instead of just calling it, tbh
    Coroutine _returnToPoolTimerCoroutine; 


    void Start()
    {
        maxHealth = 300;
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }


    void Update()
    {
        // Debug damage
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TakeDamage(25);
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

        EnemyAI ai = GetComponent<EnemyAI>();
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

        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

}
