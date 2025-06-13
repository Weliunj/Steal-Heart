using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    protected DataManager Manager;
    // ========== REFERENCE ========== 
    protected Player player;
    protected Rigidbody2D rb;
    protected Animator anim;
    [SerializeField] protected bool v2 = false;
    [SerializeField] protected bool Leader_Enemy = false;
    [SerializeField] protected bool PatrolMode = false;
    [SerializeField] protected bool JumpMode = false;

    // ========== PATROL SETTINGS ==========
    [Header("-------------Patrol Settings-----------")]
    [SerializeField] protected float movespeed = 2f;                 // T?c ?? di chuy?n khi tu?n tra
    [SerializeField] protected Transform Stay_StartPos;                      // V? trí ban ??u
    [SerializeField] protected float phamvistay;

    [SerializeField] protected float patrolDistance = 5f;             // Kho?ng cách tu?n tra
    protected Vector3 targetPos;                                      // V? trí ti?p theo
    protected bool movingToEnd;                                       // H??ng di chuy?n

    // ========== CHASE SETTINGS ==========
    [Header("--------------Chase setting-------------")]
    [SerializeField] protected float detectionRange = 3f;             // Ph?m vi phát hi?n player
    [SerializeField] protected float chaseSpeed = 3f;                // T?c ?? ?u?i
    [SerializeField] protected float attakRange = 1f;                 //Pham vi t?n công
    protected bool isChasing;                                         // Doi trang thai

    // ========== JUMP SETTINGS ==========
    [Header("--------------Jump setting-------------")]
    [SerializeField] protected float SetupY = 0.23f;
    [SerializeField] protected float StartRay;
    [SerializeField] protected float LengthRay;
    [SerializeField] protected float HightJump = 1f;

    // ========== HP ==========
    [Header("--------------------HP------------------")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int Hp;
    [SerializeField] protected GameObject Prefab;

    // ========== UI ==========
    [Header("---------------Health Bar UI------------")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image hpbar;
    [SerializeField] protected RectTransform Hpbar;

    // ========== COMBAT ==========  
    [Header("---------------Combat Settings----------")]
    [SerializeField] public int atkDMG;                      // Sát th??ng t?n công
    [SerializeField] protected float atkspeed;                 // T?c ?? ?ánh (th?i gian h?i gi?a 2 ?òn t?n công)
    [SerializeField] public float StunedPlayer;             // Th?i gian stun c?a player khi b? trúng ?òn

    [SerializeField] protected float[] onStun;

    // Bi?n n?i b? ?? x? lý tr?ng thái chi?n ??u
    protected float speed;                           // Bi?n ??m th?i gian h?i chiêu t?n công
    protected float stuned;                          // Th?i gian b? stun còn l?i c?a enemy
    protected bool Dead = false;                     // Tr?ng thái ?ã ch?t hay ch?a

    // ========== Drop ==========  
    [Header("---------------Item Drop----------")]
    public GameObject[] items;

    // ========== AUDIO & ANIMATION ==========
    [Header("--------------------Audio---------------")]
    [SerializeField] protected AudioSource[] audioSource;

    [Header("--------------- Setting ----------")]
    [SerializeField] protected float Leader_Buff = 1.2f;
    [SerializeField] protected float v2_Buff = 1.2f;
    private bool v2Activated = false;
    protected SpriteRenderer leader_Sprite;

    [HideInInspector]
    [SerializeField] public UI ui;

    public virtual void Start()
    {
        ui = FindAnyObjectByType<UI>();
        player = FindAnyObjectByType<Player>();

        //Hp
        slider.maxValue = maxHealth;
        Hp = maxHealth;

        LEADER();
    }
    public void LEADER()
    {
        leader_Sprite = GetComponent<SpriteRenderer>();
        if(Leader_Enemy && leader_Sprite != null)
        {
            leader_Sprite.color = new Color(1f, 0.5f, 0.5f); // màu đỏ nhạt
            movespeed *= Leader_Buff;
            detectionRange *= Leader_Buff;
            chaseSpeed *= Leader_Buff;
            maxHealth = Mathf.RoundToInt(maxHealth * Leader_Buff);
            atkDMG = Mathf.RoundToInt(atkDMG * Leader_Buff);
            atkspeed /= Leader_Buff;
            transform.localScale = new Vector3(transform.localScale.x * Leader_Buff, transform.localScale.y * Leader_Buff, transform.localScale.z * Leader_Buff);
        }
    }
    public void V2()
    {
        if (v2 && !v2Activated && (float)Hp / maxHealth < 0.35f)
        {
            v2Activated = true;
            leader_Sprite.color = new Color(1f, 0.5f, 0.5f); // màu đỏ nhạt
            movespeed *= v2_Buff;
            chaseSpeed *= v2_Buff;
            atkDMG = Mathf.RoundToInt(atkDMG * v2_Buff);
            atkspeed /= v2_Buff;
        }
    }

    public void ItemDrop()
    {
        int percent = Random.Range(0, 100);
        int dropCount;

        if (percent < 40)
            dropCount = 1;
        else if (percent < 60)
            dropCount = 2;
        else if (percent < 75)
            dropCount = 3;
        else if (percent < 87)
            dropCount = 4;
        else if (percent < 95)
            dropCount = 5;
        else
            dropCount = 6;

        // Nếu là Leader thì nhân đôi số lượng drop
        if (Leader_Enemy)
            dropCount *= 2;

        for (int i = 0; i < dropCount; i++)
        {
            float min = transform.position.x - 0.5f;
            float max = transform.position.x + 0.5f;
            Vector3 pos = new Vector3(Random.Range(min, max), transform.position.y + Random.Range(0.3f, 1.5f), transform.position.z);
            Instantiate(RandomItem(), pos, Quaternion.identity);
        }
    }

    public GameObject RandomItem()
    {
        int percent = Random.Range(0, 100);

        if (percent < 70)          // 0 - 69
            return items[0];                    // Coin
        else if (percent < 80)     // 70 - 79
            return items[1];                    // HP
        else if (percent < 87)     // 80 - 86
            return items[2];                    // Speed
        else if (percent < 94)     // 87 - 93
            return items[3];                    // Jump
        else                       // 94 - 99
            return items[4];                    // Strength
    }
}
