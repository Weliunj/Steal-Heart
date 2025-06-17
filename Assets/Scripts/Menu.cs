using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button play;

    private void Start()
    {
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
}
