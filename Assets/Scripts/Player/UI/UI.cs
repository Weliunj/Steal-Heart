using Unity.VisualScripting;
using UnityEngine;

public partial class UI : MonoBehaviour
{
    [Header("           --------------- Inventory ---------------")]
    public DataManager dataManager;
    private Player player;

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
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        effect_Start();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggle = !toggle; // Cập nhật toggle
            backpack.SetActive(toggle);
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
