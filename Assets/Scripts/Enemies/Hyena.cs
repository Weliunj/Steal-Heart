using System.Collections;
using UnityEngine;

public class Hyena : EnemyBase
{
    [SerializeField] protected float volumeR = 0.5f;
    [SerializeField] protected float playR = 6;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        // Compoment
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        base.Start();
        speed = atkspeed;

        if (!PatrolMode) { transform.position = Stay_StartPos.position; }
        else { transform.position = Stay_StartPos.position; targetPos = Stay_StartPos.position + Vector3.right * patrolDistance; }
        chaseSpeed += Random.Range(0f, 1.5f);


        //Audio
        audioSource[0].loop = false;      //atk
        audioSource[0].volume = 0.6f;

        audioSource[1].loop = false;      //Hit
        audioSource[1].volume = 0.4f;

        audioSource[2].volume = 1.5f;       //Run
        audioSource[2].loop = false;

        audioSource[3].volume = 0.5f;     //Free
        audioSource[3].loop = false;

        audioSource[4].volume = 0.6f;     //Dead
        audioSource[4].loop = false;

        playR = Random.Range(playR - 1f, playR + 2f);
        StartCoroutine(Free_Sound());
    }

    // Update is called once per frame
    void Update()
    {
        V2();
        HP();
        if (!Dead && Hp <= 0) //Dead
        {
            rb.linearVelocity = new Vector2(0, 0);
            Dead = true;

            anim.ResetTrigger("Hit");
            anim.SetTrigger("Dead");
            audioSource[1].Stop();
            audioSource[3].Stop();
            ItemDrop();
            Destroy(this.Prefab, 1.4f);
        }
        if (Dead) { return; }

        if (stuned > 0)
        {
            stuned -= Time.deltaTime;
        }
        else if (Hp > 0 && stuned <= 0 && player.Hp > 0)        //Player song
        {
            // Tinh khoang cach den player
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            //Neu player trong pham vi
            if (distanceToPlayer < detectionRange)
            {
                isChasing = true;
            }
            //Neu player roi khoi      
            else if (distanceToPlayer >= detectionRange * 1.1f)
            {
                isChasing = false;
            }


            if (isChasing)
            {
                ChasePlayer(distanceToPlayer);
            }
            else
            {
                Move();
            }
            if (JumpMode) { JUMP(); }
        }
    }
    public void ChasePlayer(float distanceToPlayer)
    {
        FlipSprite(player.transform.position);       //truyen vao kieu pos
        if (distanceToPlayer < attakRange)
        {
            audioSource[2].Stop();
            anim.SetBool("Walk", false);
            Atk();
        }
        else
        {
            anim.SetBool("Walk", true);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
        }
    }

    public void JUMP()
    {
        // Vị trí bắt đầu của ray
        Vector3 rayStart = transform.position + new Vector3(StartRay, SetupY, 0);

        // Kiểm tra có Ground phía trước
        RaycastHit2D frontGroundHit = Physics2D.Raycast(rayStart, Vector2.right, LengthRay, LayerMask.GetMask("Ground"));

        // Nếu có Ground trước mặt và không bị cản phía trên đầu thì nhảy
        if (frontGroundHit.collider != null)
        {
            rb.AddForce(new Vector2(0, HightJump), ForceMode2D.Impulse); // Lực nhảy có thể tùy chỉnh
        }
    }
    void Move()
    {
        if (!PatrolMode)    //dung im
        {

            //Flip
            FlipSprite(Stay_StartPos.position);
            if (Vector2.Distance(Stay_StartPos.position, transform.position) < phamvistay)
            {
                audioSource[2].Stop();
                anim.SetBool("Walk", false);
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            else
            {
                anim.SetBool("Walk", true); // Bắt đầu đi bộ quay lại StayPos
                transform.position = Vector3.MoveTowards(transform.position, Stay_StartPos.position, movespeed * Time.deltaTime); ;
            }
        }
        else
        {
            audioSource[2].Stop();
            anim.SetBool("Walk", true);
            // Move qua lai giua stast va end
            Vector2 destination = movingToEnd == true ? targetPos : Stay_StartPos.position;
            transform.position = Vector3.MoveTowards(transform.position, destination, movespeed * Time.deltaTime);
            // Doi huong khi den dich
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(destination.x, destination.y)) < 0.1f)
            {
                movingToEnd = !movingToEnd;
            }

            //Flip
            FlipSprite(destination);
        }
    }
    void Atk()
    {

        if (speed > 0)
        {
            speed -= Time.deltaTime;
        }
        else
        {
            //Atk
            anim.SetTrigger("Atk");
            audioSource[0].Play();
            //Player Hit
            speed = atkspeed;
        }
    }
    public void HP()
    {
        Vector3 bar = new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z);
        Hpbar.transform.position = bar;
        slider.value = Hp;
        // ==== Color Hp bar =====================
        if (slider.value >= (maxHealth * 0.6))
        {
            hpbar.color = Color.green;

        }
        else if (slider.value < (maxHealth * 0.6) && slider.value > (maxHealth * 0.4))
        {
            hpbar.color = new Color(1f, 0.65f, 0f); // Orange
        }
        else
        {
            hpbar.color = Color.red;
        }
    }
    void FlipSprite(Vector3 Flip)
    {
        if (Flip.x != transform.position.x)
        {
            if (Flip.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }
    public IEnumerator Free_Sound()
    {
        while (true)
        {
            yield return new WaitForSeconds(playR);
            if (!audioSource[3].isPlaying)
            {
                audioSource[3].Play();
            }
            playR = Random.Range(volumeR, volumeR + 3);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Hit
        if (collision.gameObject.CompareTag("Atk") && !Dead)
        {
            audioSource[1].Play();
            // Chỉ chơi animation Hit nếu không đang Atk
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("Atk"))
            {
                anim.SetTrigger("Hit");
            }

            if (player.atktype == "Atk1")           //Trung Atk1
            {
                this.Hp -= player.atk1_dmg;
                stuned = onStun[0];
            }
            else if (player.atktype == "Atk2")      //Trung Atk2
            {
                this.Hp -= player.atk2_dmg;
                stuned = onStun[1];
            }
            else if (player.atktype == "Atk3")      //Trung Atk3
            {
                this.Hp -= player.atk3_dmg;
                stuned = onStun[2];
                Debug.Log("-Crital-");
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            player.anim.SetTrigger("Hit");
            player.audioSources[5].Play();
            player.Hp -= atkDMG;
            player.StartCoroutine(player.dashThrougt(0.9f)); // Bat tu tam thoi
            
        }
    }
    public void OnDrawGizmos()
    {
        //Pham vi stay
        if (!PatrolMode)
        {
            Gizmos.DrawWireSphere(Stay_StartPos.position, phamvistay);
        }

        //Duong tuan tra
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Stay_StartPos.position, new Vector2(patrolDistance + Stay_StartPos.position.x, Stay_StartPos.position.y));
        }

        //Pham vi phat hien
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        //Pham vi atk
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attakRange);

        if (JumpMode)
        {
            // Ray phía trước để nhảy
            Gizmos.color = Color.green;
            Vector3 direc = transform.rotation.y == 180f ? Vector3.left : Vector3.right;
            Vector3 rayStart = transform.position + new Vector3(StartRay, SetupY, 0);
            Gizmos.DrawLine(rayStart, rayStart + direc * LengthRay);
        }
    }
}
