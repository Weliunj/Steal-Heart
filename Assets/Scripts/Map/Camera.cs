using UnityEngine;

public class Camera : MonoBehaviour
{
    public float speed = 7.4f;
    private Player player;
    private Vector3 target;
    private bool isLocked = false;  // camera có bị ghim không

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        target = player.transform.position + new Vector3(0, 3.2f, 0);
    }

    void Update()
    {
        if (!isLocked && player != null)
        {
            target = player.transform.position + new Vector3(0, 3.2f, 0);
        }

        target.z = -10f;
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }
}
