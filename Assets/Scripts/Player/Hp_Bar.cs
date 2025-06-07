using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class Player : MonoBehaviour
{
    [Range(0, 300)] private int maxHealth = 300;
    [Range(0, 300)] public int Hp;
    public Slider Hpbar;
    public Image HpImage;
    public Transform HpCanvas;
    public TMP_Text Hp_T;


    float holdTime = 0f;
    bool isHoldingH = false;

    public void HPSetup()
    {
        Hpbar.maxValue = maxHealth;
        Hp = maxHealth;
    }

    public void HPCheck()
    {
        Hpbar.value = Hp;
        HpCanvas.position = new Vector3(this.transform.position.x, this.transform.position.y + 2.5f, this.transform.position.z);

        Hp_T.text = $"{dataManager.Hp_bottle}";
        UsingHp();

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
        if(dataManager.Hp_bottle > 0 && Hp > 0)
        {
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
                    Debug.Log("Nho");
                }
                else
                {
                    Hp += 230;
                    Debug.Log("To");
                }
                dataManager.Hp_bottle--;
                if (Hp > 300) { Hp = maxHealth; }
                isHoldingH = false;
                holdTime = 0f;
            }
        }
    }
}
