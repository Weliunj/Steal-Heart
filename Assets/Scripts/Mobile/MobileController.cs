using UnityEngine;
using UnityEngine.UI;

public class MobileController : MonoBehaviour
{
    [HideInInspector] public bool isMovingLeft = false;
    [HideInInspector]public bool isMovingRight = false;
    [HideInInspector] public int move = 0;

    [HideInInspector] public bool jumpPressed = false;

    [HideInInspector] public bool Atk1Pressed = false;
    [HideInInspector] public bool Atk2Pressed = false;

    [HideInInspector] public bool InvePressed = false;
    [HideInInspector] public bool ShopPressed = false;

    [HideInInspector] public bool isDash = false;

    //---------------------------------------------------------
    [HideInInspector] public bool Speed = false;
    [HideInInspector] public bool Jump = false;
    [HideInInspector] public bool Strength = false;

    [HideInInspector] public bool item1 = false;
    [HideInInspector] public bool item2 = false;
    [HideInInspector] public bool item3 = false;
    [HideInInspector] public bool item4 = false;

    [HideInInspector] public bool isHoldingHeal = false;
    [HideInInspector] public float healHoldTime = 0f;
    void Update()
    {
        if (isMovingLeft)
            move = -1;
        else if (isMovingRight)
            move = 1;
        else
            move = 0;
    }
    public void OnLeftDown() => isMovingLeft = true;
    public void OnLeftUp() => isMovingLeft = false;
    public void OnRightDown() => isMovingRight = true;
    public void OnRightUp() => isMovingRight = false;

    // Gọi từ nút Jump
    public void OnJumpPressed() => jumpPressed = true; // Ấn 1 lần, không giữ

    public void OnAtk1Down() => Atk1Pressed = true; // Ấn 1 lần, không giữ
    public void OnAtk2Down() => Atk2Pressed = true; // Ấn 1 lần, không giữ
    public void OnAtk1Up() => Atk1Pressed = false; // Ấn 1 lần, không giữ
    public void OnAtk2Up() => Atk2Pressed = false; // Ấn 1 lần, không giữ

    public void InvenDown() => InvePressed = true;
    public void OnShopDown() => ShopPressed = true;

    public void OnDashDown() => isDash = true;
    public void OnDashUp() => isDash = false;

    public void OnSpeedPressed_Eff() => Speed = true;
    public void OnJumpPressed_Eff() => Jump = true;
    public void OnStrengthPressed_Eff() => Strength = true;

    public void OnItem1() => item1 = true;
    public void OnItem2() => item2 = true;
    public void OnItem3() => item3 = true;
    public void OnItem4() => item4 = true;

    public void OnHealDown() // Khi bấm giữ nút Heal
    {
        isHoldingHeal = true;
        healHoldTime = 0f;
    }

    public void OnHealUp() // Khi thả nút Heal
    {
        isHoldingHeal = false;
    }
}
