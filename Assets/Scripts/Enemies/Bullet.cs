using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Player player;
    Deceased dec;
    Rigidbody2D rb;

    public float speed = 1f;
    public AudioSource audiosource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        dec = FindObjectOfType<Deceased>();

        Vector2 direction = transform.up;       //Huong dau vien dan
        transform.rotation *= Quaternion.Euler(0, 0, -90);      //Chinh lai rotation
        // Đặt vận tốc cho viên đạn
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audiosource.Play();
            player.audioSources[5].Play();
            player.IsStun = dec.StunedPlayer;
            player.Hp -= dec.atkDMG;
            player.StartCoroutine(player.dashThrougt(0.9f)); // Bat tu tam thoi
            audiosource.Play();
            if (dec != null)
            {
                if (dec.burnCoroutine != null)
                {
                    dec.StopCoroutine(dec.burnCoroutine);
                }
                dec.burnCoroutine = dec.StartCoroutine(dec.Burn(Random.Range(3, 5)));
            }
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Debug.Log(collision.gameObject.name);
            audiosource.Play();
            rb.linearVelocity = Vector2.zero;
            Destroy(gameObject, 0.05f);
        }
    }
}
