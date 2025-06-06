using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    float move;
    public float speed;

    public int doubleJump =0;
    public float forcejump;

    float dashcd = 0f;
    float dashDirec;

    public string atktype;
    float atk1cd = 0f;
    float atk2cd = 0f;
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
        DASH();
        ATK();
    }
    public void MOVE()
    {
        if (dashDirec != 0 && dashcd > 1.26f) // không cho di chuyển khi đang dash
        {
            rb.linearVelocity = new Vector2(dashDirec * speed * 2f, 0);
            anim.SetTrigger("Dash");
        }
        else if (atk1cd <= 0f && atk2cd <= 0f && dashcd < 1.23f )
        {
            move = Input.GetAxisRaw("Horizontal");
            if (move != 0) { anim.SetBool("Run", true); }
            else { anim.SetBool("Run", false); }
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

            FLIP(move);
        }
        else { rb.linearVelocity = new Vector2(rb.linearVelocity.x /5, rb.linearVelocity.y / 2); anim.SetBool("Run", false); }
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
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump < 2)
        {
            doubleJump++;
            anim.SetTrigger("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcejump);
        }
    }
    public void DASH()
    {
        if(dashcd > 0) 
        { 
            dashcd -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.L) && move != 0)
            {
                dashcd = 1.5f;
                if (move < 0)
                {
                    dashDirec = -1f;
                }
                else
                {
                    dashDirec = 1f;
                }
            }
        }
    }

    public void ATK()
    {
        //Atk1
        if(atk1cd > 0) { atk1cd -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                atktype = "Atk1";
                anim.SetTrigger(atktype);
                atk1cd = 0.4f;
            }
        }
        //Atk2
        if (atk2cd > 0) 
        { 
            atk2cd -= Time.deltaTime;
            if((dashcd > 0.95f && dashcd < 1.35f))
            {
                rb.linearVelocity = new Vector2(dashDirec * speed*1.1f, 0);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K) && (dashcd > 0.8f && dashcd < 1.35f))
            {
                atk2cd = 0.70f;
                atktype = "Atk3";
                anim.SetTrigger("Atk2");
                atk2cd = 0.7f;
                Debug.Log("Crital");
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                atktype = "Atk2";
                anim.SetTrigger(atktype);
                atk2cd = 0.70f;
            }

        }
    }
}
