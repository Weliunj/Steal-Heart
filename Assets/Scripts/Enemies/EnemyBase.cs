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
    protected Transform Player;
    [SerializeField ]protected bool PatrolMode = false;
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
    [SerializeField] protected float SetupY = 0.5f;
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
    [SerializeField] protected int atkDMG;                      // Sát th??ng t?n công
    [SerializeField] protected float atkspeed;                 // T?c ?? ?ánh (th?i gian h?i gi?a 2 ?òn t?n công)
    [SerializeField] protected float StunedPlayer;             // Th?i gian stun c?a player khi b? trúng ?òn

    [SerializeField] protected float[] onStun;

    // Bi?n n?i b? ?? x? lý tr?ng thái chi?n ??u
    protected float speed;                           // Bi?n ??m th?i gian h?i chiêu t?n công
    protected float stuned;                          // Th?i gian b? stun còn l?i c?a enemy
    protected bool Dead = false;                     // Tr?ng thái ?ã ch?t hay ch?a

    // ========== AUDIO & ANIMATION ==========
    [Header("--------------------Audio---------------")]
    [SerializeField] protected AudioSource[] audioSource;
}
