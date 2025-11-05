using UnityEngine;

public class Movement : MonoBehaviour
{


    public float speed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
        transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z + speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
        transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }


    }
}
