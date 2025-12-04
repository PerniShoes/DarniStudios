using UnityEngine;

public enum AttackTypes
{
    Lunge,
    Melee
}

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
    public bool isRanged;
    public AttackTypes attackType;
    public float damageTriggerNormalizedTime;

    [Header("References")]
    public HealthBar healthBar;
    public Animator animator;

    [Header("Utils")]
    public float destroyDelay;

}
