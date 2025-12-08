using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{

    public Rigidbody body;
    public Animator animator;

    [Header("Run settings")]
    public float acceleration;
    public float deceleration;
    public float topSpeed;
    private readonly float rotationSpeed = 15f;

    [Header("Dash settings")]
    public int totalDashes;
    public float dashDuration;
    public float dashSpeed;
    public float dashCooldown;
    public float delayBetweenDashes;
    private bool isDashing = false;
    [SerializeField] private TrailRenderer trailRenderer;

    private int currentDashCharges;
    private bool canDash = true;
    private float dashRecharge = 0f;

    [Header("Dash UI")]
    public DashChargesSetup dashUI;
    public List<Image> dashCharges = new List<Image>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dashUI.SetSlotAmount(totalDashes);
        currentDashCharges = totalDashes;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing)
        {
            float speed = new Vector3(body.linearVelocity.x, 0f, body.linearVelocity.z).magnitude / 8f;
            animator.SetFloat("runAnimSpeed", Mathf.Clamp(speed, 1f, 10f));
        }
        HandleInput();
        UpdateDashUI();
        if (currentDashCharges < totalDashes)
        {
            dashRecharge += Time.deltaTime;

            if (dashRecharge >= dashCooldown)
            {
                currentDashCharges++;
                dashRecharge = 0f;
            }
        }
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
        if (flatVector.magnitude > topSpeed && !isDashing)
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

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }


    }
    private IEnumerator Dash()
    {
        if (currentDashCharges <= 0 || !canDash) yield break;
        if (!isDashing)
        {

        }

        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;
        currentDashCharges -= 1;

        Vector3 dashDir = transform.forward;

        float timePassedSinceDash = 0f;

        while (timePassedSinceDash < dashDuration)
        {
            body.linearVelocity = dashDir * dashSpeed;
            timePassedSinceDash += Time.deltaTime;
            yield return null;
        }
        isDashing = false;
        trailRenderer.emitting = false;
        body.linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(delayBetweenDashes);
        canDash = true;
    }

    private void UpdateDashUI()
    {
        bool skipRest = false;
        for (int i = 0; i < dashCharges.Count; i++)
        {
            if (skipRest)
            {
                dashCharges[i].fillAmount = 0f;
                continue;
            }
            if (i < currentDashCharges)
            {
                dashCharges[i].fillAmount = 1f;
            }
            else
            {
                dashCharges[i].fillAmount = dashRecharge/dashCooldown;
                skipRest = true;
            }
        }
    }


}
