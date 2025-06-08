using System.Collections;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    public DataManager dataManager;
    public Rigidbody2D rb;
    public Animator anim;

    private bool dead = false   ;
    float move;
    public float speed;

    public int doubleJump =0;
    public float forcejump;

    public float dashcd = 0f;
    float dashDirec;

    public string atktype;
    float atk1cd = 0f;
    float atk2cd = 0f;

    public AudioSource[] audioSources;      //0: Run, 1: Atk1, 2: Atk2, 3: Atk3, 4: Jump, 5: Hit
    public AudioSource usingItems;

    void Start()
    {
        HPSetup();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        foreach (AudioSource source in audioSources) { source.playOnAwake = false; }
        audioSources[0].loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0 && !dead) { dead = true;  anim.SetTrigger("Dead"); audioSources[4].Stop(); audioSources[4].Play(); }
        if (dead) return; // Nếu đã chết rồi, không xử lý gì tiếp

        HPCheck();
        BUFFS();

        MOVE();
        DASH();
        ATK();
    }
    public void MOVE()
    {
        if(holdTime <= 0f)
        {
            if (dashDirec != 0 && dashcd > 1.26f) // không cho di chuyển khi đang dash
            {
                rb.linearVelocity = new Vector2(dashDirec * speed * 2f * (1.15f * speed_Buff), 0);
                anim.SetTrigger("Dash");
            }
            else if (atk1cd <= 0f && atk2cd <= 0.2f && dashcd < 1.26f)
            {
                move = Input.GetAxisRaw("Horizontal");
                if (move != 0) { anim.SetBool("Run", true); audioSources[0].Play(); }
                else { anim.SetBool("Run", false); audioSources[0].Pause(); }

                rb.linearVelocity = new Vector2(move * speed * speed_Buff, rb.linearVelocity.y);

                FLIP(move);
            }
            else { rb.linearVelocity = new Vector2(rb.linearVelocity.x / 5, rb.linearVelocity.y); anim.SetBool("Run", false); }
            JUMP();
        }
        else { anim.SetBool("Run", false); rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); }
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
        int countjump = Jump_Buff + 2;
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump < countjump)
        {
            dashcd = 0.1f;
            audioSources[4].Play();
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
            if (Input.GetKeyDown(KeyCode.L) && move != 0 && atk2cd <= 0.2f)
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
                StartCoroutine(dashThrougt(0.2f));
            }
        }
    }
    public IEnumerator dashThrougt(float dura)
    {   
        // Tạm tắt va chạm giữa Player và Enemy
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true
        );
        // Chờ dash kết thúc
        yield return new WaitForSeconds(dura);

        // Bật lại va chạm
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            false
        );
    }
    float direcatk3;
    public void ATK()
    {
        //Atk1
        if(atk1cd > 0) { atk1cd -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                audioSources[1].Stop(); audioSources[1].Play();
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
                rb.linearVelocity = new Vector2(direcatk3 * speed*1.1f, rb.linearVelocity.y*0.5f);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K) && (dashcd > 0.8f && dashcd < 1.4f))
            {
                direcatk3 = dashDirec;
                audioSources[3].Stop(); audioSources[3].Play();
                atk2cd = 0.70f;
                atktype = "Atk3";
                anim.SetTrigger("Atk2");
                atk2cd = 0.7f;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                audioSources[2].Stop(); audioSources[2].Play();
                atktype = "Atk2";
                anim.SetTrigger(atktype);
                atk2cd = 0.70f;
            }

        }
    }
}
