using System.Collections;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    [Header("           -------------- GENERAL ---------------")]

    [Header("-------------Display")]
    public DataManager dataManager;
    UI inventory;
    public Rigidbody2D rb;
    public Animator anim;

    private bool dead = false;
    float move;
    public int doubleJump =0;
    public float dashcd = 0f;
    float dashDirec;

    public float IsStun = 0f;

    [HideInInspector] public bool isDashingThroughBullet = false;
    [Header("-------------Settings")]
    public float speed;
    public float forcejump;

    [Header("-------------Audio")]
    public AudioSource[] audioSources;      //0: Run, 1: Atk1, 2: Atk2, 3: Atk3, 4: Jump, 5: Hit

    void Start()
    {
        dataManager.LastSceneName = SceneManager.GetActiveScene().name;

        inventory = FindAnyObjectByType<UI>();
        BOD = FindAnyObjectByType<BOD>();

        Hp_Start();
        Ignore_Start();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        foreach (AudioSource source in audioSources) { source.playOnAwake = false; }
        audioSources[0].loop = true;
        audioSources[0].volume= 20f;
        audioSources[4].volume = 50f;

    }

    // Update is called once per frame
    void Update()
    {
        HP_Update();
        if (Hp <= 0 && !dead)
        { 
            dead = true; anim.ResetTrigger("Hit") ; 
            anim.SetTrigger("Dead"); 
            audioSources[4].Stop(); 
            audioSources[4].Play();

            // Trừ tài nguyên
            dataManager.Coin_Quan = Mathf.FloorToInt(dataManager.Coin_Quan * 0.7f);
            dataManager.Hp_bottle = Mathf.FloorToInt(dataManager.Hp_bottle * 0.7f);
            dataManager.Jump_bottle = Mathf.FloorToInt(dataManager.Jump_bottle * 0.7f);
            dataManager.Speed_bottle = Mathf.FloorToInt(dataManager.Speed_bottle * 0.7f);
            dataManager.Strength_bottle = Mathf.FloorToInt(dataManager.Strength_bottle * 0.7f);
        }
        if (dead) { rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); return; }// Nếu đã chết rồi, không xử lý gì tiếp
        if(IsStun > 0) 
        { 
            IsStun -= Time.deltaTime;
            anim.SetBool("Run", false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            MOVE();
            DASH();
            ATK();
        }
    }
    public void MOVE()
    {
        if(holdTime <= 0f)
        {
            if (dashDirec != 0 && dashcd > 0.75f) // không cho di chuyển khi đang dash
            {
                rb.linearVelocity = new Vector2(dashDirec * speed * 2f * BOD.Slow_Strength, 0);
                anim.SetTrigger("Dash");
            }
            else if (atk1cd <= 0f && atk2cd <= 0.2f && dashcd < 0.75f)
            {
                move = Input.GetAxisRaw("Horizontal");
                if (move != 0) { anim.SetBool("Run", true); audioSources[0].Play(); }
                else { anim.SetBool("Run", false); audioSources[0].Pause(); }

                rb.linearVelocity = new Vector2(move * speed * inventory.speed_Buff * BOD.Slow_Strength, rb.linearVelocity.y);

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
        int count = 2 + inventory.Jump_Buff;
        if (Input.GetKeyDown(KeyCode.Space) && doubleJump < count)
        {
            audioSources[4].Play();
            doubleJump++;
            anim.SetTrigger("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcejump * BOD.Slow_Strength);
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
                dashcd = 1f;
                if (move < 0)
                {
                    dashDirec = -1f;
                }
                else
                {
                    dashDirec = 1f;
                }
                StartCoroutine(dashThrougt(0.4f));
            }
        }
    }
    public IEnumerator dashThrougt(float dura)
    {
        isDashingThroughBullet = true;
        // Tạm tắt va chạm giữa Player và Enemy
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Enemy"),
            true
        );
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Bullet"),
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
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Bullet"),
            false
        );
        isDashingThroughBullet = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LoadScene_TriggerEnter(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
