using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOD : EnemyBase
{
    [Header("-------------Unique-----------")]
    public bool isV2 = false;
    public bool isV3 = false;

    public GameObject Atk2Prefab;
    public int Atk2Damage;
    public float Atk2Speed = 1f;
    public Vector3 Atk2_Scale;

    private Coroutine skill2Coroutine;
    private Coroutine skill3Coroutine;

    private bool IsUsingAnySkill => skill2Coroutine != null || skill3Coroutine != null;
    public float CD_Skill = 5;
    public float Cd_SkillPrivate = 5f;
    [Header("------------- Skill 2 Settings -------------")]
    private bool OnSkill2 = false;
    public float skill2_Speed = 1f;
    public float skill2CooldownMin = 5f;
    public float skill2CooldownMax = 10f;
    public int skill2ProjectileCount = 5;

    private float skill2CooldownTimer = 0f;
    private float skill2LockDuration = 0.4f;

    [Header("------------- Skill 3 Settings -------------")]
    public GameObject[] skill3RandomZones; // [xMin, xMax, yMin, yMax]
    public float skill3_Speed = 3f;

    public int soluong = 10;
    public int dot = 3;


    public float skill3CooldownMin = 5f;
    public float skill3CooldownMax = 10f;
    public int skill3BaseCount = 3;
    public int skill3MaxCount = 15;

    private float skill3CooldownTimer = 0f;
    [Header("------------- SPAWM -------------")]
    public GameObject[] enemies;
    public int spawnCount = 1;
    public float Spawn_cd = 20f;
    private float Spawn_cdPrivate = 20f;
    [Header("------------- Teleport -------------")]
    public Transform[] pos;
    public float cdTele;
    private float cdtelePrivate;
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
    public IEnumerator clear()
    {
        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        HP();
        if (!Dead && Hp <= 0) //Dead
        {
            rb.linearVelocity = new Vector2(0, 0);
            Dead = true;

            StopAllCoroutines();
            anim.ResetTrigger("Atk1");
            anim.ResetTrigger("Atk2");
            anim.SetTrigger("Dead");
            if (!audioSource[4].isPlaying)
            {
                audioSource[4].Play();
            }
            ItemDrop();
            StartCoroutine(clear());
        }
        if (Dead) { return; }



        if (stuned > 0)
        {
            stuned -= Time.deltaTime;
        }

        else if (Hp > 0 && stuned <= 0 && player.Hp > 0)        //Player song
        {
            // Tinh khoang cach den player
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position + new Vector3(0, 0.5f, 0));

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

            // Đặt bên trong Update() trước đoạn kiểm tra isChasing
            if (skill2CooldownTimer > 0) skill2CooldownTimer -= Time.deltaTime;
            if (skill3CooldownTimer > 0) skill3CooldownTimer -= Time.deltaTime;

            SPAWN();
            TELE();

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
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0, 0.5f, 0), chaseSpeed * Time.deltaTime);
            }
        }

        if (Cd_SkillPrivate > 0) Cd_SkillPrivate -= Time.deltaTime;
        if (!IsUsingAnySkill && Cd_SkillPrivate <= 0) // Chỉ khi không có skill nào đang chạy
        {
            if (skill2CooldownTimer <= 0 && skill3CooldownTimer <= 0)
            {
                // Cả hai skill đều sẵn sàng, chọn ngẫu nhiên
                if (Random.value < 0.5f) // 50% cơ hội dùng Skill 2, 50% dùng Skill 3
                {
                    TryUseSkill2();
                }
                else
                {
                    TryUseSkill3();
                }
            }
            else if (skill2CooldownTimer <= 0)
            {
                TryUseSkill2(); // Chỉ Skill 2 sẵn sàng
            }
            else if (skill3CooldownTimer <= 0)
            {
                TryUseSkill3(); // Chỉ Skill 3 sẵn sàng
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

    void SPAWN()
    {
        if (Spawn_cdPrivate > 0)
        {
            Spawn_cdPrivate -= Time.deltaTime;
        }
        else
        {
            for (int s = 0; s < spawnCount; s++)
            {
                int randomS = Random.Range(0, enemies.Length);
                Instantiate(enemies[randomS], new Vector3(
                    transform.position.x + Random.Range(-1f, 1f),
                    transform.position.y + Random.Range(1f, 4f),
                    0), Quaternion.identity);
            }
            Spawn_cdPrivate = Random.Range(Spawn_cd, Spawn_cd + 5);
        }
    }
    void TELE()
    {
        if(cdtelePrivate > 0) { cdtelePrivate-= Time.deltaTime; }
        else
        {
            int rd = Random.Range(0, pos.Length);
            this.transform.position = pos[rd].position;
            cdtelePrivate = cdTele + Random.Range(-1f, 3f);
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
            // Nếu đang dùng skill 2 hoặc 3 thì hủy
            if (skill2Coroutine != null)
            {
                StopCoroutine(skill2Coroutine);
                skill2Coroutine = null;
                OnSkill2 = false;
            }
            if (skill3Coroutine != null)
            {
                StopCoroutine(skill3Coroutine);
                skill3Coroutine = null;
            }

            //Atk
            anim.SetTrigger("Atk1");
            StartCoroutine(fixSound());

            //Player Hit
            speed = atkspeed + Random.Range(-0.3f, 0.5f);
        }
    }

    public IEnumerator fixSound()
    {
        yield return new WaitForSeconds(0.23f);
        audioSource[0].Play();
    }
    void TryUseSkill2()
    {
        if (skill2CooldownTimer > 0 || IsUsingAnySkill) return;

        skill2Coroutine = StartCoroutine(Skill2Coroutine());
        skill2CooldownTimer = Random.Range(skill2CooldownMin, skill2CooldownMax);
    }


    IEnumerator Skill2Coroutine()
    {
        OnSkill2 = true;
        for (int i = 0; i < skill2ProjectileCount; i++)
        {
            skill2LockDuration = 0.4f;

            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Atk1"))
            {
                anim.SetTrigger("Atk2");
            }

            yield return new WaitForSeconds(0.3f); // tránh trùng animation
            Instantiate(Atk2Prefab, player.transform.position + new Vector3(0, 0.65f, 0), Quaternion.identity);
            yield return new WaitForSeconds(skill2_Speed);
        }

        OnSkill2 = false;
        skill2Coroutine = null;
        Cd_SkillPrivate = Random.Range(CD_Skill, CD_Skill + 5f);
    }



    void TryUseSkill3()
    {
        if (skill3CooldownTimer > 0 || IsUsingAnySkill) return;

        skill3Coroutine = StartCoroutine(Skill3Coroutine());
        skill3CooldownTimer = Random.Range(skill3CooldownMin, skill3CooldownMax);
    }


    IEnumerator Skill3Coroutine()
    {
        float currentSoluong = soluong;

        for (int i = 0; i < dot; i++)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Atk1"))
            {
                anim.SetTrigger("Atk2");
            }

            yield return new WaitForSeconds(0.3f); // khớp animator

            for (int j = 0; j < currentSoluong; j++)
            {
                int zoneIndex = Random.Range(0, skill3RandomZones.Length);
                float x = Random.Range(skill3RandomZones[0].transform.position.x, skill3RandomZones[1].transform.position.x);
                float y = Random.Range(skill3RandomZones[2].transform.position.y, skill3RandomZones[3].transform.position.y);
                Vector3 spawnPos = new Vector3(x, y, 0f);

                Instantiate(Atk2Prefab, spawnPos, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(0.01f, 0.13f));
            }

            yield return new WaitForSeconds(skill3_Speed);

            currentSoluong += Random.Range(5, 10); // tăng thêm mỗi đợt
        }

        skill3Coroutine = null;
        Cd_SkillPrivate = Random.Range(CD_Skill, CD_Skill + 5f);
    }



    public void EnterPhase2()
    {
        if (isV2) return;
        isV2 = true;

        skill2ProjectileCount = Random.Range(12, 17);
        dot = Mathf.CeilToInt(dot * 1.2f);
        chaseSpeed *= 1.2f;
        soluong = Mathf.CeilToInt(soluong * 1.2f); // tăng số lượng skill3 spawn mỗi đợt
        skill2CooldownMin *= 0.8f;
        skill2CooldownMax *= 0.8f;
        skill3CooldownMin *= 0.8f;
        skill3CooldownMax *= 0.8f;

        skill2_Speed = 0.75f;
        skill3_Speed = 2.3f;
        cdTele = 13f;

        atkDMG = Mathf.CeilToInt(atkDMG * 1.3f);
        Atk2Damage = Random.Range(45, 50);
        Atk2Speed = 0.6f;
        CD_Skill = 3f;
        atkspeed = 2.7f;
        spawnCount = 2;
        Spawn_cd = 11f;
        transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        // Đổi màu cam đậm
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0f);

        Debug.Log("=== Entered Phase 2 ===");
    }

    public void EnterPhase3()
    {
        if (isV3) return;
        isV3 = true;

        skill2ProjectileCount = Random.Range(18, 25);
        soluong = Mathf.CeilToInt(soluong * 1.3f); // tăng số lượng skill3 spawn mỗi đợt
        dot = Mathf.CeilToInt(dot * 1.3f);
        chaseSpeed *= 1.3f;

        skill2CooldownMin *= 0.7f;
        skill2CooldownMax *= 0.7f;
        skill3CooldownMin *= 0.7f;
        skill3CooldownMax *= 0.7f;

        skill2_Speed = 0.5f;
        skill3_Speed = 1.5f;
        cdTele = 10f;

        atkspeed = 2.4f;
        Atk2Speed = 0.4f;
        atkDMG = Mathf.CeilToInt(atkDMG * 1.5f);
        Atk2Damage = Random.Range(50, 55);
        CD_Skill = 1f;

        spawnCount = 3;
        Spawn_cd = 7f;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        // Đổi màu đỏ
        GetComponent<SpriteRenderer>().color = Color.red;
        Debug.Log("=== Entered Phase 3 ===");
    }


    public void HP()
    {
        slider.value = Hp;

        if (!isV2 && Hp <= maxHealth * 0.6f)
        {
            EnterPhase2();
        }
        if (!isV3 && Hp <= maxHealth * 0.3f)
        {
            EnterPhase3();
        }

        // ==== Color Hp bar ============================
        if (slider.value >= (maxHealth * 0.6f))
        {
            hpbar.color = Color.green;
        }
        else if (slider.value < (maxHealth * 0.6f) && slider.value > (maxHealth * 0.3f))
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
            player.Hp -= atkDMG + Random.Range(0, 10);
            player.IsStun = StunedPlayer;
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
