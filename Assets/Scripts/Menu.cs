using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button play;
    public GameObject guide;
    private bool isGuideShown = false;
    private void Start()
    {
        guide.SetActive(false);
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

    

    public void ToggleGuide()
    {
        isGuideShown = !isGuideShown;
        guide.SetActive(isGuideShown);
    }

}
