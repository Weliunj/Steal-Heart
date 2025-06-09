using Unity.VisualScripting;
using UnityEngine;

public partial class UI : MonoBehaviour
{
    [Header("           --------------- Toggle ---------------")]
    public DataManager dataManager;
    public Player player;

    public GameObject backpack;
    bool toggle = false;
    private float AutoClose = 5f;

    AudioSource audioSource;    //Using

    public void Start()
    {
        backpack.SetActive(toggle);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        EFFECT();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
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
                AutoClose = 5f;
            }
        }

        if(player.Hp > 0)
        {
            BUFFS();
        }
    }
}
