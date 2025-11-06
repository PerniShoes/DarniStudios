using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;      
    public float moveSpeed = 3.5f;
    public float rotateSpeed = 5f;
    public float attackRange = 2f;
    public float turnSpeed = 10f;

    public Animator animator;




    public void Start()
    {
        if (!player) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.sqrMagnitude < 0.0001f) return;

        Quaternion look = Quaternion.LookRotation(toPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    void Update()
    {
        // if there is no player, do nothing
        if (player == null) return;

        // calculate direction to the player (only on the XZ plane)
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        // rotate smoothly towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

        // calculate distance to the player
        float distance = direction.magnitude;

        // if player is far → run
        if (distance > attackRange)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            
            animator.SetBool("isMoveing", true);
            animator.SetBool("isAttack", false);
        }
        else
        {
            // if close → attack
            animator.SetBool("isMoveing", false);
            animator.SetBool("isAttack", true);
        }
    }
}
