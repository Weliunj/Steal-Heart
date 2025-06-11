using System.Collections;
using UnityEngine;

public class BOD : EnemyBase
{
    [Header("-------------Unique-----------")]
    private bool isV2 = false;
    private bool isV3 = false;
    [Header("------------- Skill 2 Settings -------------")]
    public GameObject skill2Prefab;
    public int skill2Damage = 100;
    public float skill2Speed = 1f;
    public float skill2CooldownMin = 5f;
    public float skill2CooldownMax = 10f;
    public int skill2ProjectileCount = 5;

    private float skill2CooldownTimer = 0f;
    private float skill2LockDuration = 0.4f;

    [Header("------------- Skill 3 Settings -------------")]
    public GameObject[] skill3RandomZones; // [xMin, xMax, yMin, yMax]
    public float skill3Speed = 3f;
    public float skill3CooldownMin = 5f;
    public float skill3CooldownMax = 10f;
    public int skill3BaseCount = 3;
    public int skill3MaxCount = 15;

    private float skill3CooldownTimer = 0f;
    private int currentSkill3Count = 3;
    private float skill3LockDuration = 0.4f;


    [Header("-------------Slow_Effects")]
    public float Slow_Strength = 1;
    public float Slow_dura;
    public Coroutine slowCoroutine;       // Theo dõi coroutine hiện tại


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
        audioSource[0].volume = 4.5f;

        audioSource[1].loop = false;      //Hit
        audioSource[1].volume = 0.6f;

        audioSource[2].volume = 0.6f;       //Run
        audioSource[2].loop = false;

        audioSource[3].volume = 1.4f;     //Free
        audioSource[3].loop = false;

        audioSource[4].volume = 0.8f;     //Dead
        audioSource[4].loop = false;
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


            anim.ResetTrigger("Atk1");
            anim.ResetTrigger("Atk2");
            anim.SetTrigger("Dead");
            if (!audioSource[4].isPlaying)
            {
                audioSource[4].Play();
            }
            ItemDrop();
            Destroy(this.Prefab, 1.5f);
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
        FlipSprite(player.transform.position);

        if (distanceToPlayer < attakRange)
        {
            audioSource[2].Stop();
            anim.SetBool("Walk", false);

            Atk1();
            TryUseSkill2();
            TryUseSkill3();
        }
        else
        {
            anim.SetBool("Walk", true);

            if (skill2LockDuration > 0)
            {
                skill2LockDuration -= Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
            }
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
                rb.linearVelocity = new Vector2(0, -1f);
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

    public void Atk1()
    {
        if (speed > 0)
        {
            speed -= Time.deltaTime;
        }
        else
        {
            //Atk
            anim.SetTrigger("Atk1");
            audioSource[0].Play();
            //Player Hit
            speed = atkspeed;
        }
        

    }
    void TryUseSkill2()
    {
        if (skill2CooldownTimer > 0)
        {
            skill2CooldownTimer -= Time.deltaTime;
            return;
        }

        anim.SetTrigger("Atk2");
        StartCoroutine(Skill2Coroutine());
        skill2CooldownTimer = Random.Range(skill2CooldownMin, skill2CooldownMax);
    }

    IEnumerator Skill2Coroutine()
    {
        for (int i = 0; i < skill2ProjectileCount; i++)
        {
            skill2LockDuration = 0.4f;
            anim.SetTrigger("Atk2");
            yield return new WaitForSeconds(0.3f); // tránh bị trùng animation
            Instantiate(skill2Prefab, player.transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
            yield return new WaitForSeconds(skill2Speed);
        }
    }


    void TryUseSkill3()
    {
        if (skill3CooldownTimer > 0)
        {
            skill3CooldownTimer -= Time.deltaTime;
            return;
        }

        anim.SetTrigger("Atk2");
        StartCoroutine(Skill3Coroutine());
        skill3CooldownTimer = Random.Range(skill3CooldownMin, skill3CooldownMax);
    }

    IEnumerator Skill3Coroutine()
    {
        int spawnCount = currentSkill3Count;

        currentSkill3Count = Mathf.Min(currentSkill3Count + 3, skill3MaxCount);

        for (int i = 0; i < spawnCount; i++)
        {
            skill2LockDuration = 0.4f;
            anim.SetTrigger("Atk2");
            yield return new WaitForSeconds(0.3f); // tránh bị trùng animator

            float x = Random.Range(skill3RandomZones[0].transform.position.x, skill3RandomZones[1].transform.position.x);
            float y = Random.Range(skill3RandomZones[2].transform.position.y, skill3RandomZones[3].transform.position.y);
            Vector3 spawnPos = new Vector3(x, y, 0f);

            Instantiate(skill2Prefab, spawnPos, Quaternion.identity); // dùng chung prefab
            yield return new WaitForSeconds(skill3Speed);
        }
    }


    public void HP()
    {
        slider.value = Hp;

        // ==== Buff theo phần trăm máu ================
        if (!isV2 && Hp <= maxHealth * 0.6f)
        {
            isV2 = true;
            skill2Speed *= 1.2f;
            skill3Speed *= 1.2f;
            atkspeed *= 0.8f;   // tấn công nhanh hơn
            Debug.Log("==> BOD V2 Activated!");
        }

        if (!isV3 && Hp <= maxHealth * 0.3f)
        {
            isV3 = true;
            skill2Speed *= 1.3f;
            skill3Speed *= 1.3f;
            atkspeed *= 0.7f;   // tấn công càng nhanh hơn
            skill2ProjectileCount += 2;
            skill3BaseCount += 2;
            Debug.Log("==> BOD V3 Activated!");
        }

        // ==== Color Hp bar ============================
        if (slider.value >= (maxHealth * 0.6f))
        {
            hpbar.color = Color.green;
        }
        else if (slider.value < (maxHealth * 0.6f) && slider.value > (maxHealth * 0.4f))
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

    public IEnumerator SlowEffects(float speed, float dura)
    {

        ui.Slow.SetActive(true);
        Slow_Strength = 0.45f;
        yield return new WaitForSeconds(dura);
        Slow_Strength = 1f;
        ui.Slow.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Atk") && !Dead)
        {
            audioSource[1].Play();
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
