using UnityEngine;

public class Ghim_Cam : MonoBehaviour
{
    Player player;
    Camera cam;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        cam = FindAnyObjectByType<Camera>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.SetTarget(transform.position); // Ghim vào vị trí hiện tại
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.Unlock(); // Bỏ ghim, cho theo player lại
        }
    }
}
