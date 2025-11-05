using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float moveSpeed = 3.5f;
    public float rotateSpeed = 10f;

    public Transform player;

    void Update()
    {
        if (player == null) return;


        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);


        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
    // DODAC ANIMACJE DO ENEMY ABY PODARZAL ZA GRACZEM A NIE 



}
