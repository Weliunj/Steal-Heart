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
        speedAtk = boss.skill2Speed;
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
            player.Hp -= boss.skill2Damage;
            player.StartCoroutine(player.dashThrougt(0.9f)); // Bat tu tam thoi

            if (boss != null)
            {
                if (boss.slowCoroutine != null)
                {
                    boss.StopCoroutine(boss.slowCoroutine);
                }
                boss.slowCoroutine = boss.StartCoroutine(boss.SlowEffects(boss.Slow_Strength, boss.Slow_dura));
            }
        }
    }
}
