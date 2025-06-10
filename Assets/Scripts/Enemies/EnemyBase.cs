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
    [HideInInspector]
    [SerializeField] public UI ui;

    public virtual void Start()
    {
        ui = FindAnyObjectByType<UI>();
        player = FindAnyObjectByType<Player>();
    }
    public void ItemDrop()
    {
        int percent = Random.Range(0, 100); // Theo %

        int dropCount = 0;
        if (percent > 55)
        {
            dropCount = 1;
        }
        else if (percent > 30)
        {
            dropCount = 2;
        }
        else
        {
            dropCount = 3;
        }

        for (int i = 0; i < dropCount; i++)
        {
            float min = transform.position.x - 0.5f;
            float max = transform.position.x + 0.5f;
            Vector3 pos = new Vector3(Random.Range(min, max), transform.position.y + Random.Range(0f, 1f), transform.position.z);

            Instantiate(RandomItem(), pos, Quaternion.identity);
        }
    }
    public GameObject RandomItem()
    {
        int percent = Random.Range(0, 100);

        if (percent < 40)          // 0 - 39
            return items[0];                    // Coin
        else if (percent < 60)     // 40 - 59
            return items[1];                    // HP
        else if (percent < 75)     // 60 - 74
            return items[2];                    // Speed
        else if (percent < 90)     // 75 - 89
            return items[3];                    // Jump
        else                       // 90 - 99
            return items[4];                    // Strength
    }
}
