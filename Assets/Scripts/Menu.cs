using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
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
}
