using System;
using UnityEngine;

public partial class Shop : MonoBehaviour
{
    [Header("           --------------- Shop ---------------")]
    [Header("-------------Ui")]
    public GameObject Key;
    public GameObject Shop_Panel;
    public DataManager dataManager;

    // ========== SHOP DATA ==========
    private UI ui;
    private bool toggleShop;

    private float timebuying = 0.5f;
    private AudioSource m_AudioSource;
    public void Start()
    {
        ui = FindAnyObjectByType<UI>();

        m_AudioSource = GetComponent<AudioSource>();
        Key.SetActive(false);
        Shop_Panel.SetActive(false);
        toggleShop = false;
    }

    public void Update()
    {
        if (Key.activeSelf)
        {
            HandleToggleShop();
        }

        if (Shop_Panel.activeSelf)
        {
            HandleBuyItems();
        }
    }

    public void HandleToggleShop()
    {
        //Chi cho phep buy khi dang o shop
        if(Key.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                toggleShop = !toggleShop;
                Shop_Panel.SetActive(toggleShop);
            }
        }
        else
        {
            Shop_Panel.SetActive(false );
        }
    }
    public void HandleBuyItems()
    {
        ui.OpenBackpack();
        if(timebuying > 0) { timebuying -= Time.deltaTime; }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) BuyItem(15, ref dataManager.Hp_bottle);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) BuyItem(8, ref dataManager.Speed_bottle);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) BuyItem(5, ref dataManager.Jump_bottle);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) BuyItem(10, ref dataManager.Strength_bottle);
        }
    }
    void BuyItem(int cost, ref int itemCount)
    {

        if (dataManager.Coin_Quan >= cost)
        {
            dataManager.Coin_Quan -= cost;
            itemCount++;

            if (m_AudioSource != null)
            {
                m_AudioSource.Play();
            }
        }
        timebuying = 0.5f;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Key.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Key.SetActive(false);
            Shop_Panel.SetActive(false);
            toggleShop = false;
            ui.CloseBackPack();     //Player thoat
        }
    }
}
