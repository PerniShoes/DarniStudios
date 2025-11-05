using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Movement : MonoBehaviour
{

    public Rigidbody body;
    public Animator animator;
    public float acceleration;
    public float deceleration;

    public float topSpeed;
    private readonly float rotationSpeed = 12f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {

        float speed = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z).magnitude/8f;
        animator.SetFloat("runAnimSpeed", Mathf.Clamp(speed,1f,10f));
        
    }

    private void FixedUpdate()
    {
        SetRotatationAndVelocity();

    }

    private void SetRotatationAndVelocity()
    {
        Vector3 velocity = body.linearVelocity;
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotation
        if (inputDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            body.MoveRotation(Quaternion.Slerp(body.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
        }

        // Velocity
        if (inputDir.sqrMagnitude > 0.01f)
        {
            velocity += inputDir * (acceleration * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);
            flatVel = Vector3.MoveTowards(flatVel, Vector3.zero, deceleration * Time.fixedDeltaTime);
            velocity.x = flatVel.x;
            velocity.z = flatVel.z;
        }

        Vector3 flatVector = new Vector3(velocity.x, 0f, velocity.z);
        if (flatVector.magnitude > topSpeed)
        {
            flatVector = flatVector.normalized * topSpeed;
        }

        body.linearVelocity = new Vector3(flatVector.x, velocity.y, flatVector.z);

        if (flatVector.magnitude > 0.5f)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

    }

}
