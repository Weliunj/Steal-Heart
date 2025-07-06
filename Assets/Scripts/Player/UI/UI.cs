using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class UI : MonoBehaviour
{
    [Header("           --------------- Inventory ---------------")]
    public DataManager dataManager;
    private Player player;
    private MobileController mobileController;

    public GameObject backpack;
    bool toggle = false;
    private float AutoClose = 0f;

    AudioSource audioSource;    //Using

    public void Start()
    {
        toggle = true;
        backpack.SetActive(toggle);
        audioSource = GetComponent<AudioSource>();
        player = FindAnyObjectByType<Player>();
        mobileController = FindAnyObjectByType<MobileController>();

        audioSource.loop = false;
        audioSource.playOnAwake = false;
        effect_Start();

        Jump_Buff = 0;
        speed_Buff = 1f;
        Stre_Buff = 1f;
        buffs_Update();
    }
    public void Update()
    {
        // Toggle khi nhấn nút (chỉ toggle 1 lần mỗi khi nhấn)
        if (mobileController.InvePressed && player.Hp > 0)
        {
            toggle = !toggle;
            backpack.SetActive(toggle);
            AutoClose = 8f;
            mobileController.InvePressed = false; // RESET để không bị spam
        }

        //Tu dong dong inventory
        if (backpack.activeSelf)
        {
            if(AutoClose > 0) { AutoClose -= Time.deltaTime; }
            else
            {
                backpack.SetActive(false);
                toggle = false; // ĐỒNG BỘ LẠI trạng thái
                AutoClose = 8f;
            }
        }

        if(player.Hp > 0)
        {
            buffs_Update();
        }
    }

    public void OpenBackpack()
    {
        toggle = true;
        backpack.SetActive(true);
        AutoClose = 8f;
    }
    public void CloseBackPack()
    {
        toggle = false;
        backpack.SetActive(false);
    }

}
