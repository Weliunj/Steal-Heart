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
            player.anim.SetBool("Jump_E", false);
            player.doubleJump = 0;
        }

        if (collision.gameObject.CompareTag("CheckClimb"))
        {
            player.rb.linearVelocity = Vector2.zero;
            player.anim.SetTrigger("Wall");
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckJump"))
        {
            player.anim.SetBool("Jump_E", true);
            
        }
        if (collision.gameObject.CompareTag("CheckClimb"))
        {
            player.anim.SetBool("Jump_E", true);
        }
    }
}
