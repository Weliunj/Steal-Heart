using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform player;
    public float speed = 7.4f;
    public Vector3 target;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        speed = 7.4f;
    }

    void Update()
    {
        target = player.position + new Vector3(0, 3.2f, 0);
        target.z = -10f;
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }
}
