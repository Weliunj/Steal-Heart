using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player;
    public float speed;
    public Vector3 target;


    void Update()
    {
        target = player.position + new Vector3(0, 3.2f, 0);
        target.z = -10f;
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }
}
