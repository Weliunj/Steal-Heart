using UnityEngine;

public partial class Player : MonoBehaviour
{
    [Header("           -------------- ATK ---------------")]
    public string atktype;

    [Header("-------------Settings")]
    public int Atk1_Damage;
    public int Atk2_Damage;
    public int Atk3_Damage;
    float atk1cd = 0f;
    float atk2cd = 0f;


    float direcatk3;


    public int atk1_dmg => Mathf.RoundToInt(Atk1_Damage * inventory.Stre_Buff);
    public int atk2_dmg => Mathf.RoundToInt(Atk2_Damage * inventory.Stre_Buff);
    public int atk3_dmg => Mathf.RoundToInt(Atk3_Damage * inventory.Stre_Buff);

    public void ATK()
    {

        //Atk1
        if (atk1cd > 0) { atk1cd -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                audioSources[1].Stop(); audioSources[1].Play();
                atktype = "Atk1";
                anim.SetTrigger(atktype);
                atk1cd = 0.5f;
            }
        }
        //Atk2
        if (atk2cd > 0)
        {
            atk2cd -= Time.deltaTime;
            if ((dashcd > 0.55f && dashcd < 0.85f))
            {
                rb.linearVelocity = new Vector2(direcatk3 * speed * 1.1f, rb.linearVelocity.y * 0.5f);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K) && (dashcd > 0.55f && dashcd < 0.85f))
            {
                direcatk3 = dashDirec;
                audioSources[3].Stop(); audioSources[3].Play();
                atk2cd = 0.70f;
                atktype = "Atk3";
                anim.SetTrigger("Atk2");
                atk2cd = 0.7f;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                audioSources[2].Stop(); audioSources[2].Play();
                atktype = "Atk2";
                anim.SetTrigger(atktype);
                atk2cd = 0.70f;
            }

        }
    }
}
