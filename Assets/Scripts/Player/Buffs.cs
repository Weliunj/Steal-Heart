using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public partial class Player : MonoBehaviour
{
    [Header("           -------------- BUFF ---------------")]
    [Header("-------------Display")]
    public TMP_Text speed_T;
    public TMP_Text Jump_T;
    public TMP_Text Stre_T;

    private Coroutine jumpCoroutine;
    private Coroutine streCoroutine;
    private Coroutine speedCoroutine;

    private float speed_Buff = 1;
    private float speed_Timer = 15f;

    private int Jump_Buff = 1;
    private float Jump_Timer = 20f;

    private float Stre_Buff = 1f;         //He so nhan
    private float Stre_Timer =15f;

    [Header("-------------Settings")]
    public float speed_cd = 0f;
    public float Jump_cd = 0f;
    public float Stre_cd = 0f;

    IEnumerator speed_eff()
    {
        speed_Buff = 1.3f;
        yield return new WaitForSeconds(speed_Timer);
        speed_Buff = 1f;
    }

    IEnumerator Jump_eff()
    {
        Jump_Buff = 1;
        yield return new WaitForSeconds(Jump_Timer);
        Jump_Buff = 0;
    }

    IEnumerator Stre_eff()
    {
        Stre_Buff = 1.5f;
        yield return new WaitForSeconds(Stre_Timer);
        Stre_Buff = 1f;
    }
    public void BUFFS()
    {
        Jump_T.text = $"{dataManager.Jump_bottle}";
        speed_T.text = $"{dataManager.Speed_bottle}";
        Stre_T.text = $"{dataManager.Strength_bottle}";

        //Test
        if ( Hp > 0)
        {
            if (speed_cd >= 0) { speed_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.U) && dataManager.Speed_bottle > 0)
                {
                    usingItems.Play();
                    dataManager.Speed_bottle--;
                    speed_cd = 10f;

                    if (speedCoroutine != null) StopCoroutine(speedCoroutine);
                    speedCoroutine = StartCoroutine(speed_eff());
                }
            }

            if (Jump_cd >= 0) { Jump_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.I) && dataManager.Jump_bottle > 0)
                {
                    usingItems.Play();
                    dataManager.Jump_bottle--;
                    Jump_cd = 15f;

                    if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
                    jumpCoroutine = StartCoroutine(Jump_eff());
                }
            }

            if (Stre_cd >= 0) { Stre_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.O) && dataManager.Strength_bottle > 0)
                {
                    usingItems.Play();
                    dataManager.Strength_bottle--;
                    Stre_cd = 15f;

                    if (streCoroutine != null) StopCoroutine(streCoroutine);
                    streCoroutine = StartCoroutine(Stre_eff());
                }
            }
        }
    }
}
