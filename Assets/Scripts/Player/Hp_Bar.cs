using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    [Range(0, 300)] private int maxHealth = 300;
    [Range(0, 300)] public int Hp;
    public Slider Hpbar;
    public Image HpImage;
    public Transform HpCanvas;
    

    public void HPSetup()
    {
        Hpbar.maxValue = maxHealth;
        Hp = maxHealth;
    }

    public void HPCheck()
    {
        Hpbar.value = Hp;
        HpCanvas.position = new Vector3(this.transform.position.x, this.transform.position.y + 2.5f, this.transform.position.z);

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
}
