using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dead : MonoBehaviour
{
    public Button replay;
    public Button menu;
    private Player player;
    public DataManager dataManager;
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        player = FindAnyObjectByType<Player>();

        if (replay != null)
            replay.onClick.AddListener(OnReplay);

        if (menu != null)
            menu.onClick.AddListener(OnMenu);
    }

    private void OnReplay()
    {
        // Giảm tài nguyên 25% nhưng không bao giờ xuống dưới 0
        dataManager.Coin_Quan = Mathf.Max(0, Mathf.FloorToInt(dataManager.Coin_Quan * 0.75f));
        dataManager.Hp_bottle = Mathf.Max(0, Mathf.FloorToInt(dataManager.Hp_bottle * 0.75f));
        dataManager.Jump_bottle = Mathf.Max(0, Mathf.FloorToInt(dataManager.Jump_bottle * 0.75f));
        dataManager.Speed_bottle = Mathf.Max(0, Mathf.FloorToInt(dataManager.Speed_bottle * 0.75f));
        dataManager.Strength_bottle = Mathf.Max(0, Mathf.FloorToInt(dataManager.Strength_bottle * 0.75f));

        dataManager.Hp_temp = player.maxHealth;
        player.Hp = player.maxHealth;
        // Load lại scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnMenu()
    {
        // Load scene menu (giả sử là scene 0)
        SceneManager.LoadScene(0);
    }
}
