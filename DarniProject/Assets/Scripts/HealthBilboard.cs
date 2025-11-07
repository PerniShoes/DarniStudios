using UnityEngine;

public class HealthBillboard : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        // If camera is not set by Dev set it automaticly
        if (cam == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                cam = mainCam.transform;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // Always turn to camera
        transform.LookAt(transform.position + cam.forward);
    }
}

