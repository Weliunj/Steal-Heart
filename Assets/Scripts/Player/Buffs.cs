using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public partial class Player : MonoBehaviour
{
    private float speed_Buff = 1;
    private float speed_Timer = 8f;
    public float speed_cd = 0f;
    public TMP_Text speed_T;
    IEnumerator speed_eff()
    {
        speed_Buff = 1.3f;
        yield return new WaitForSeconds(speed_Timer);
        speed_Buff = 1f;
    }
    //-----------------------------------------------------------------
    private int Jump_Buff = 1;
    private float Jump_Timer = 8f;
    public float Jump_cd = 0f;
    public TMP_Text Jump_T;
    IEnumerator Jump_eff()
    {
        Jump_Buff = 1;
        yield return new WaitForSeconds(Jump_Timer);
        Jump_Buff = 0;
    }
    public void BUFFS()
    {
        Jump_T.text = $"{dataManager.Jump_bottle}";
        speed_T.text = $"{dataManager.Speed_bottle}";

        //Test
        if ( Hp > 0)
        {
            if (speed_cd >= 0) { speed_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.N) && dataManager.Speed_bottle > 0)    //Speed
                {
                    usingItems.Play();
                    dataManager.Speed_bottle--;
                    speed_cd = 10f;
                    StopCoroutine(speed_eff());
                    StartCoroutine(speed_eff());
                }
            }

            if (Jump_cd >= 0) { Jump_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.M) && dataManager.Jump_bottle > 0)    //Jump
                {
                    usingItems.Play();
                    dataManager.Jump_bottle--;
                    Jump_cd = 10f;
                    StopCoroutine(Jump_eff());
                    StartCoroutine(Jump_eff());
                }
            }
        }
    }
}
