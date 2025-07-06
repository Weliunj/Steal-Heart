using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO; // Đảm bảo có using này

public class Menu : MonoBehaviour
{
    public DataManager dataManager; // Gắn ScriptableObject vào Inspector
    public Button play;

    public GameObject Guidepanel;
    public Toggle Giude;
    public bool Ison = false;
    private void Start()
    {
        Giude.onValueChanged.AddListener(GuideToggle); // thay vì tự toggle thủ công
        Guidepanel.SetActive(false);

        if (play != null)
        {
            play.onClick.AddListener(PLAY);
        }
        else
        {
            Debug.LogWarning("Button 'play' is not assigned in the Inspector!");
        }
    }


    public void PLAY()
    {
        SceneManager.LoadScene(1);
    }

    // Sửa GuideToggle nhận tham số từ Toggle
    public void GuideToggle(bool isOn)
    {
        Guidepanel.SetActive(isOn); // không cần biến Ison nữa
    }

    public void Continue()
    {
        string path = Application.persistentDataPath + "/saveData.txt";

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            string[] parts = data.Split('\t');

            if (parts.Length >= 7)
            {
                // Gán giá trị vào ScriptableObject
                dataManager.LastSceneName = parts[0];
                dataManager.Coin_Quan = int.Parse(parts[1]);
                dataManager.Hp_bottle = int.Parse(parts[2]);
                dataManager.Jump_bottle = int.Parse(parts[3]);
                dataManager.Speed_bottle = int.Parse(parts[4]);
                dataManager.Strength_bottle = int.Parse(parts[5]);
                dataManager.Hp_temp = int.Parse(parts[6]);

                Debug.Log("Đã load data từ file:");
                Debug.Log("Scene: " + dataManager.LastSceneName);
                Debug.Log("Coin: " + dataManager.Coin_Quan);
                Debug.Log("HP bottle: " + dataManager.Hp_bottle);
                Debug.Log("Jump: " + dataManager.Jump_bottle);
                Debug.Log("Speed: " + dataManager.Speed_bottle);
                Debug.Log("Strength: " + dataManager.Strength_bottle);
                Debug.Log("Hp_temp: " + dataManager.Hp_temp);

                // Load scene đã lưu
                SceneManager.LoadScene(dataManager.LastSceneName);
            }
            else
            {
                Debug.LogWarning("❌ Dữ liệu file không đầy đủ!");
            }
        }
        else
        {
            Debug.LogWarning("❌ Không tìm thấy file saveData.txt!");
        }
    }

    public void QuitGame()
    {
        // Đảm bảo game tiếp tục trước khi quit
        Time.timeScale = 1f;

        #if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #else
                    Application.Quit();
        #endif
    }
}
