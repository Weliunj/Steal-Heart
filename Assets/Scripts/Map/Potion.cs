using UnityEngine;

public class Position : MonoBehaviour
{
    [Header("     1: Hp" +
         "\n      2: Speed" +
         "\n      3: Jump" +
         "\n      4: Strength")]
    public int select;
    private AudioSource audioSource;

    private bool fixbug = false;
    public DataManager dataManager;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.volume = 2.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !fixbug)
        {
            fixbug = true;
            audioSource.Play();
            if(dataManager != null)
            {
                if(select == 1)
                {
                    dataManager.Hp_bottle++;
                }
                else if(select == 2)
                {
                    dataManager.Speed_bottle--;
                }
                else if (select == 3)
                {
                    dataManager.Jump_bottle++;
                }
                else if (select == 4)
                {
                    dataManager.Strength_bottle--;
                }
            }
            Destroy(this.gameObject, 0.17f);
        }
    }
}
