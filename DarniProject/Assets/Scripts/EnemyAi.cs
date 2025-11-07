using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;      
    public float moveSpeed = 3.5f; 
    public float attackRange = 2f; 
    public Animator animator;      

    public bool isDead = false;    

    void Update()
    {
        // IF Ded Calm Down 
        if (isDead) return;

        if (player == null) return;

        // Move to player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        // If Player is far from atack go to him
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

        // Turn to player
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
