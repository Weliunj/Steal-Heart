using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckJump"))
        {
            player.anim.SetBool("Jump", false);
            player.doubleJump = 0;
        }
    }
}
