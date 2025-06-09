using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;
    bool Onground = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckClimb") && !Onground)
        {
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, -0.3f);
            player.anim.SetBool("Wall", true);
            player.doubleJump = 1;
            player.rb.gravityScale = 0f;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckJump"))
        {
            Onground = true;
            player.anim.SetBool("Wall", false);
            player.anim.SetBool("Jump_E", false);
            player.doubleJump = 0;
        }
        if (collision.gameObject.CompareTag("CheckClimb") && !Onground)
        {
            player.dashcd = 0;
            player.anim.SetBool("Jump_E", false);
            player.anim.SetBool("Wall", true);

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckJump"))
        {
            Onground = false;
            player.rb.gravityScale = 2.3f;
            player.anim.SetBool("Jump_E", true);
        }
        if (collision.gameObject.CompareTag("CheckClimb"))
        {
            player.anim.SetBool("Wall", false);
            player.anim.SetBool("Jump_E", true);
            player.dashcd = 0f;
            player.doubleJump = 0;
            player.rb.gravityScale = 2.3f;
        }
    }
}
