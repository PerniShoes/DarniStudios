using UnityEngine;

public class MouseHelper : MonoBehaviour 
{
    public GameObject mouseTarget;
    public float targetHeight;

    private void Update()
    {

        Vector3 mousePos = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Plane plane = new Plane(Vector3.up, new Vector3(0, targetHeight, 0));
        if (plane.Raycast(ray, out float distance))
        {
            mouseTarget.transform.position = ray.GetPoint(distance);
        }
        else
        {
            mouseTarget.transform.position = Vector3.zero;
        }
    }
}
