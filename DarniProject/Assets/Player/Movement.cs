using UnityEngine;

public class Movement : MonoBehaviour
{

    public Animator animator;
    public float speed = 30f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // speed = 31f;
        animator.SetBool("isSpawned", true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z + speed * Time.deltaTime);
            animator.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            animator.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
            animator.SetBool("isMoving", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            animator.SetBool("isMoving", true);
        }


    }
}
