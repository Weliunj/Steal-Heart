using UnityEngine;

public class Ghim_Cam : MonoBehaviour
{
    Player player;
    Camera cam;
    AudioSource bossAu;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        cam = FindAnyObjectByType<Camera>();
        bossAu = GetComponent<AudioSource>();
        bossAu.loop = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cam.SetTarget(transform.position); // Ghim vào vị trí hiện tại
            bossAu.Play();
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
