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

    public float dashcd = 1f;

    public string atktype;
    float atk2cd = 1f;
    float atk3cd = 3f;
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
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump < 2)
        {
            doubleJump++;
            anim.SetTrigger("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcejump);
        }
    }
    public void DASH()
    {
        if( dashcd > 0) { dashcd -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.L) && move != 0)
            {
                float direc = move;
                rb.linearVelocity = new Vector2(direc * (speed*1.5f), rb.linearVelocity.y);
                dashcd = 1f;
                Debug.Log("Da dash");
            }
        }
    }
    public void ATK()
    {
        //Atk1
        if (Input.GetKeyDown(KeyCode.J))
        {
            atktype = "Atk1";
            anim.SetTrigger(atktype);
        }

        //Atk2
        if (atk2cd > 0) { atk2cd -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                atktype = "Atk2";
                anim.SetTrigger(atktype);
                atk2cd = 1f;
            }
        }

        //Atk special
        if (atk3cd > 0) { atk3cd -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.K) && (dashcd > 0.2f && dashcd < 0.7f))
            {
                atktype = "Atk3";
                anim.SetTrigger("Atk2");
                atk3cd = 3f;
                Debug.Log("Crital");
            }
        }
    }
}
