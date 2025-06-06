using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    float move;
    float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MOVE();

    }
    public void MOVE()
    {
        move = Input.GetAxisRaw("Horizontal");
        if(move > Mathf.Abs(0.1f)) { anim.SetBool("Run", true); }
        else { anim.SetBool("Run", false); }
        FLIP(move);
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }
    public void FLIP(float move)
    {
        if(move != 0)
        {
            if(move > 0.1f) { transform.rotation = Quaternion.Euler(0, 0, 0); }
            else { transform.rotation = Quaternion.Euler(0, 180, 0); }
        }
    }
}
