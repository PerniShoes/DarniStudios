using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3.5f;
    public float attackRange = 2f;
    public Animator animator;

    public bool isDead = false;

    public EnemyHP enemyHP;

    void Awake()
    {
        enemyHP = GetComponent<EnemyHP>();
    }
    void Start()
    { 
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (animator == null)
            animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (isDead || player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

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

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}
