using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth;
    public int currentHealth;
    public bool isDead;
    public bool isBoss;
    public float moveSpeed;
    public float attackRange;
    public int damage;

    [Header("References")]
    public HealthBar healthBar;
    public Animator animator;

    [Header("Utils")]
    public float destroyDelay;

}
