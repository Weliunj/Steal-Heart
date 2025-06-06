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
            player.anim.SetBool("Jump_E", true);
            player.doubleJump = 0;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckJump"))
        {
            player.anim.SetBool("Jump_E", false);
            player.anim.SetTrigger("Jump");
            
        } 
    }
}
