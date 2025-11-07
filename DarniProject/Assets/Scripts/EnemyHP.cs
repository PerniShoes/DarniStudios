using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("References")]
    public HealthBar healthBar;
    public Animator animator;

    [Header("EXP Settings")]
    public GameObject expPrefab; 
    public int expAmount = 10;   

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        // Test DMG (Delete after testing)
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Stop AI 
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        // Play Animation of Death
        animator.SetBool("isDead", true);

        // Delete hp bar
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        // BUM BUM Enemy
        Destroy(gameObject, 1f);

        if (expPrefab != null)
        {
            Instantiate(expPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

    }
}
