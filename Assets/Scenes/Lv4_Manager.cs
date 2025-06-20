using UnityEngine;

public class Lv4_Manager : MonoBehaviour
{
    public GameObject[] Start_End;
    public GameObject spawnEvent;
    public GameObject daidien;
    Player player;
    public AudioSource[] AudioSource;

    private bool isDone =false;
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        spawnEvent.SetActive(false);
    }

    private void Update()
    {
        if (player != null)
        {
            if(player.transform.position.x > Start_End[0].transform.position.x && !isDone)     //Bat dau
            {
                if(spawnEvent != null)
                {
                    spawnEvent.SetActive(true);
                }
                if (AudioSource[0].isPlaying == false)
                {
                    AudioSource[0].Play();
                }
            }
        }

        if(player.transform.position.x >= Start_End[1].transform.position.x  && player.transform.position.y <= Start_End[1].transform.position.y)
        {
            isDone = true;
            AudioSource[0].Stop();
            spawnEvent.SetActive(false);
        }

        if(player.transform.position.x < daidien.transform.position.x + 1f)
        {
            player.audioSources[5].Play();
            player.Hp = 0;
        }
    }

}
