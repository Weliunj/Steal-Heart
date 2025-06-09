using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public partial class Player : MonoBehaviour
{
    [Header("           -------------- HP ---------------")]
    [Header("-------------Display")]
    [Range(0, 300)] protected int maxHealth = 300;
    [Range(0, 300)] public int Hp;
    public Slider Hpbar;
    public Image HpImage;
    public TMP_Text Hp_T;
    public float Hp_cd = 0f;
    public GameObject Hp_Eff;
    [Header("-------------Stam")]
    public Slider DashBar;


    float holdTime = 0f;
    bool isHoldingH = false;

    public void Setup()
    {
        Hp_Eff.SetActive(true);
        Hpbar.maxValue = maxHealth;
        Hp = maxHealth;
        DashBar.maxValue = 1f;
    }

    public void HPCheck()
    {
        Hpbar.value = Hp;
        DashBar.value = dashcd;

        Hp_T.text = $"{dataManager.Hp_bottle}";
        if(Hp_cd > 0) 
        { 
            Hp_Eff.SetActive (false);
            Hp_cd -= Time.deltaTime; 
        }
        else 
        { 
            Hp_Eff.SetActive(true) ;
            UsingHp(); 
        }

        float healthPercent = (float)Hp / maxHealth;
        if (healthPercent > 0.7f)
        {
            HpImage.color = Color.green;
        }
        else if (healthPercent > 0.3f)
        {
            HpImage.color = new Color(1f, 0.65f, 0f); // Orange
        }
        else
        {
            HpImage.color = Color.red;
        }
    }
    public void UsingHp()
    {
        if (dataManager.Hp_bottle <= 0 || Hp <= 0 || Hp_cd > 0)  return;        //delta.time
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHoldingH = true; //kiem tra t gian
            holdTime = 0f; //reset bo dem
        }

        // Đang giữ phím H
        if (isHoldingH && Input.GetKey(KeyCode.H))
        {
            holdTime += Time.deltaTime;
        }

        // Thả phím H ra
        if (isHoldingH && Input.GetKeyUp(KeyCode.H))
        {
            if (holdTime < 1.5f)
            {
                Hp += 130;
                Debug.Log("HP + 130");
            }
            else
            {
                Hp += 230;
                Debug.Log("HP + 230");
            }

            dataManager.Hp_bottle--;
            inventory.usingItems.Play();
            if (Hp > 300) { Hp = maxHealth; }
            isHoldingH = false;
            holdTime = 0f;
            Hp_cd = 5f;
        }
    }
}
