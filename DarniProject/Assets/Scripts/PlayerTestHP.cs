using UnityEngine;
using TMPro;

public class PlayerTestHP : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public HealthBar healthBar;
    public TextMeshProUGUI healthText; 

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        UpdateHealthText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); 

        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
    }
    public void SetMaxHealth(int value)
    {
        healthBar.SetMaxHealth(value);
        UpdateHealthText();
    }
    void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = $"{currentHealth} / {maxHealth}";
    }

}
