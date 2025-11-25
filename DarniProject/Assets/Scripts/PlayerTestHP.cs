using UnityEngine;

public class PlayerTestHP : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }
    void Update()
    {
        // I ADDED THIS TO CHECK IF HEALTH BAR WORKS YOU CAN DELETE IT //
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            TakeDamage(20);
        }
     
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

    }
}
