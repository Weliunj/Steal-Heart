using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    public DataManager dataManager;
    public int value = 1;

    private bool fixbug = false;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 3f;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !fixbug)
        {
            fixbug = true;
            audioSource.Play();
            dataManager.Coin_Quan += value ;
            Destroy(this.gameObject, 0.15f);
        }
    }
}
