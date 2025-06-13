using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public partial class UI : MonoBehaviour
{
    [Header("           --------------- Items ---------------")]
    [Header("-------------Display")]
    public TMP_Text coin_T;
    public AudioSource usingItems;

    public TMP_Text speed_T;
    public float speed_Buff = 1;
    [SerializeField] private float speed_Timer = 15f;
    public float speed_cd = 0f;
    private Coroutine speedCoroutine;

    public TMP_Text Jump_T;
    public int Jump_Buff = 0;
    [SerializeField] private float Jump_Timer = 20f;
    public float Jump_cd = 0f;
    private Coroutine jumpCoroutine;

    public TMP_Text Stre_T;
    public float Stre_Buff = 1f;
    [SerializeField]private float Stre_Timer =15f;
    public float Stre_cd = 0f;
    private Coroutine streCoroutine;

    IEnumerator Speed_eff()
    {
        speed_Buff = 1.2f;
        Speed.SetActive(true);
        yield return new WaitForSeconds(speed_Timer);
        speed_Buff = 1f;
        Speed.SetActive(false);
    }

    IEnumerator Jump_eff()
    {
        Jump_Buff = 1;
        Jump.SetActive(true);
        yield return new WaitForSeconds(Jump_Timer);
        Jump_Buff = 0;
        Jump.SetActive(false);
    }

    IEnumerator Stre_eff()
    {
        Stre_Buff = 1.5f;
        Stre.SetActive(true);
        yield return new WaitForSeconds(Stre_Timer);
        Stre_Buff = 1f;
        Stre.SetActive(false);
    }
    public void buffs_Update()
    {
        coin_T.text = $"{dataManager.Coin_Quan}";
        Jump_T.text = $"{dataManager.Jump_bottle}";
        speed_T.text = $"{dataManager.Speed_bottle}";
        Stre_T.text = $"{dataManager.Strength_bottle}";

        //Test
        if ( player.Hp > 0)
        {
            if (speed_cd >= 0) { speed_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.U) && dataManager.Speed_bottle > 0)
                {
                    dataManager.Speed_bottle--;
                    speed_cd = 10f;
                    audioSource.Play();
                    if (speedCoroutine != null) StopCoroutine(speedCoroutine);
                    speedCoroutine = StartCoroutine(Speed_eff());
                }
            }

            if (Jump_cd >= 0) { Jump_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.I) && dataManager.Jump_bottle > 0)
                {
                    dataManager.Jump_bottle--;
                    Jump_cd = 15f;
                    audioSource.Play();
                    if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
                    jumpCoroutine = StartCoroutine(Jump_eff());
                }
            }

            if (Stre_cd >= 0) { Stre_cd -= Time.deltaTime; }
            else
            {
                if (Input.GetKeyDown(KeyCode.O) && dataManager.Strength_bottle > 0)
                {
                    dataManager.Strength_bottle--;
                    Stre_cd = 15f;
                    audioSource.Play();
                    if (streCoroutine != null) StopCoroutine(streCoroutine);
                    streCoroutine = StartCoroutine(Stre_eff());
                }
            }
        }
    }
}
