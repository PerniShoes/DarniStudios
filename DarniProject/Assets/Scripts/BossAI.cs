using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("Boss Settings")]
    public int maxHealth = 1000;
    private int currentHealth;
    private bool isDead = false;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;

    [Header("References")]
    public Transform player;
    public Animator animator;
    public HealthBar healthBar; 

    void Start()
    {
        currentHealth = maxHealth;

        
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

      
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

       
        if (healthBar == null)
            healthBar = Object.FindFirstObjectByType<HealthBar>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

       
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

       
        if (distance > attackRange)
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

        if (animator != null)
            animator.SetBool("isDead", true);

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
