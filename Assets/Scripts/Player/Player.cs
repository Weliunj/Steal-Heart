using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;

    float move;
    public float speed;

    public int doubleJump =0;
    public float forcejump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MOVE();

    }
    public void MOVE()
    {
        move = Input.GetAxisRaw("Horizontal");
        if(move != 0) { anim.SetBool("Run", true); }
        else { anim.SetBool("Run", false); }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        FLIP(move);
        JUMP();
    }
    public void FLIP(float move)
    {
        if(move != 0)
        {
            if(move > 0.1f) { transform.rotation = Quaternion.Euler(0, 0, 0); }
            else { transform.rotation = Quaternion.Euler(0, 180, 0); }
        }
    }
    public void JUMP()
    {
        if (Input.GetKey(KeyCode.Space) && doubleJump < 2)
        {
            doubleJump++;
            anim.SetBool("Jump", true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcejump);
        }
    }
}
