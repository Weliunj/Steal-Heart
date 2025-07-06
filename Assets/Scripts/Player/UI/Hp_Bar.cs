using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public partial class Player : MonoBehaviour
{
    [Header("           -------------- HP ---------------")]
    [Header("-------------Display")]
    [HideInInspector] [Range(0, 300)] public int maxHealth = 300;
    [Range(0, 300)] public int Hp;
    public Slider Hpbar;
    public TMP_Text Hp_T;
    public float Hp_cd = 0f;
    public GameObject Hp_Eff;
    [Header("-------------Stam")]
    public Slider DashBar;


    float holdTime = 0f;
    bool isHoldingH = false;

    public void Hp_Start()
    {
        Hp_Eff.SetActive(true);
        Hpbar.maxValue = maxHealth;
        Hp = maxHealth;
        Hp = dataManager.Hp_temp;

        DashBar.maxValue = 1f;
    }

    public void HP_Update()
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
            if(dataManager.Hp_bottle > 0)
            {
                Hp_Eff.SetActive(true);
            }
            UsingHp(); 
        }

        
    }
    public void UsingHp()
    {
        if (dataManager.Hp_bottle <= 0 || Hp <= 0 || Hp_cd > 0) return;

        // ==== KEYBOARD: giữ phím H ====
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHoldingH = true;
            holdTime = 0f;
        }

        if (isHoldingH && Input.GetKey(KeyCode.H))
        {
            holdTime += Time.deltaTime;
        }

        if (isHoldingH && Input.GetKeyUp(KeyCode.H))
        {
            int healAmount = holdTime < 1.5f ? 130 : 230;
            ApplyHeal(healAmount);
            isHoldingH = false;
            holdTime = 0f;
        }

        // ==== MOBILE: giữ nút Heal ====
        if (mobile.isHoldingHeal)
        {
            mobile.healHoldTime += Time.deltaTime;
        }
        else if (mobile.healHoldTime > 0f) // Khi thả nút
        {
            int healAmount = mobile.healHoldTime < 1.5f ? 130 : 230;
            ApplyHeal(healAmount);
            mobile.healHoldTime = 0f;
        }
    }
    private void ApplyHeal(int amount)
    {
        Hp += amount;
        Debug.Log($"Mobile: HP + {amount}");
        dataManager.Hp_bottle--;
        inventory.usingItems.Play();
        if (Hp > maxHealth) Hp = maxHealth;
        Hp_cd = 5f;
    }

}
