using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public DataManager dataManager;

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

        if (Input.GetKeyDown(KeyCode.H) && dataManager.Hp_bottle > 0)
        {
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.H) && dataManager.Jump_bottle > 0)
        {
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.H) && dataManager.Speed_bottle > 0)
        {
            audioSource.Play();
        }
    }
}
