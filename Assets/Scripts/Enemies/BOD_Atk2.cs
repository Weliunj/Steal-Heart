using System.Collections;
using UnityEngine;

public class BOD_Atk2 : MonoBehaviour
{
    BOD boss;
    Player player;
    UI ui;

    private Animator anim;
    public AudioSource audiosource;
    private float speedAtk;
    public void Start()
    {
        player = FindAnyObjectByType<Player>();
        boss = FindAnyObjectByType<BOD>();
        ui = FindAnyObjectByType<UI>();
        anim = GetComponent<Animator>();
        speedAtk = boss.Atk2Speed;

        audiosource.volume = 3f;

        float v2 = Random.Range(1.1f, 1.25f);
        float v3 = Random.Range(1.15f, 1.4f);
        if (boss.isV2)
        {
            transform.localScale = new Vector3(v2,v2);
            speedAtk = boss.Atk2Speed - Random.Range(0.1f, 0.2f);
        }
        else if (boss.isV3)
        {
            transform.localScale = new Vector3(v3, v3);
            speedAtk = boss.Atk2Speed - Random.Range(0.2f, 0.3f);
        }
    }
    public void Update()
    {
        if(speedAtk > 0)
        {
            speedAtk -= Time.deltaTime;
        }
        else
        {
            anim.SetTrigger("Atk");
            audiosource.Play();
            speedAtk = 999;
            StartCoroutine(LifeTime(1f));
        }
    }

    public IEnumerator LifeTime(float dura)
    {
        yield return new WaitForSeconds(dura);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audiosource.Play();
            player.audioSources[5].Play();
            player.IsStun = boss.StunedPlayer;
            player.Hp -= boss.Atk2Damage;
            player.StartCoroutine(player.dashThrougt(0.9f)); // Bat tu tam thoi

                if (player.slowCoroutine != null)
                {
                    player.StopCoroutine(player.slowCoroutine);
                }
                player.slowCoroutine = player.StartCoroutine(player.SlowEffects(player.Slow_dura));
        }
    }
}
