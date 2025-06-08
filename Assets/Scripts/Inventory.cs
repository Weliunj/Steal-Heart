using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public DataManager dataManager;
    public Player player;

    public GameObject backpack;
    bool toggle = false;

    AudioSource audioSource;    //Using

    public void Start()
    {
        backpack.SetActive(toggle);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            toggle = !toggle; // Cập nhật toggle
            backpack.SetActive(toggle);
        }

        if (Input.GetKeyUp(KeyCode.H) && player.Hp_cd <= 0 && (dataManager.Hp_bottle > 0 && player.Hp > 0 && player.Hp_cd <= 0))
        {
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.M) && dataManager.Jump_bottle > 0 && player.Jump_cd <= 0)
        {
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.N) && dataManager.Speed_bottle > 0 && player.speed_cd <= 0)
        {
            audioSource.Play();
        }
    }
}
