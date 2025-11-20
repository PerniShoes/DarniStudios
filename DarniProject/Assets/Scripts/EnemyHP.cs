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
    public event EnemyDeathEvent OnDeath;

    void Start()
    {
        maxHealth = 300;
        currentHealth = maxHealth;

        // Automatycznie przypisz pasek życia, jeśli nie został ustawiony w Inspectorze
        if (healthBar == null)
            healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }


    void Update()
    {
        // Test damage (delete after tests)
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

        // Stop AI
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null) ai.enabled = false;

        // Death Anim
        if (animator != null)
            animator.SetBool("isDead", true);

        // DeleteHealthBar
        if (healthBar != null)
            Destroy(healthBar.gameObject);

        // Let other scripts know that enemy is dead
        OnDeath?.Invoke(this);

        // Delete Ragdoll After few Seconds
        Destroy(gameObject, 1.5f);
    }
}
