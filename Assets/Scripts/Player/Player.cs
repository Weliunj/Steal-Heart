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
    public GameObject DeadPanel;
    float move;
    public int doubleJump =0;
    public float dashcd = 0f;
    float dashDirec;

    public float IsStun = 0f;

    [Header("-------------Slow_Effects")]
    public Coroutine slowCoroutine;       // Theo dõi coroutine hiện tại
    public float Slow_Strength = 1;
    public float Slow_dura;

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
        if(BOD != null)
        {
            BOD = FindAnyObjectByType<BOD>();
        }

        Hp_Start();
        Ignore_Start();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        DeadPanel.SetActive(false);

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
            DeadPanel.SetActive(true);
            dead = true; anim.ResetTrigger("Hit") ; 
            anim.SetTrigger("Dead"); 
            audioSources[4].Stop(); 
            audioSources[4].Play();

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
                rb.linearVelocity = new Vector2(dashDirec * speed * 2f * Slow_Strength, 0);
                anim.SetTrigger("Dash");
            }
            else if (atk1cd <= 0f && atk2cd <= 0.2f && dashcd < 0.75f)
            {
                move = Input.GetAxisRaw("Horizontal");
                if (move != 0) { anim.SetBool("Run", true); audioSources[0].Play(); }
                else { anim.SetBool("Run", false); audioSources[0].Pause(); }

                rb.linearVelocity = new Vector2(move * speed * inventory.speed_Buff * Slow_Strength, rb.linearVelocity.y);

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

            float temp = Slow_Strength != 1 ? 0.9f : 1;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcejump * temp);
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
    public IEnumerator SlowEffects(float dura)
    {

        inventory.Slow.SetActive(true);
        Slow_Strength = 0.75f;
        yield return new WaitForSeconds(dura);
        Slow_Strength = 1f;
        inventory.Slow.SetActive(false);
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
