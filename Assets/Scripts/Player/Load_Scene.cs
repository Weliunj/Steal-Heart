using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Player : MonoBehaviour
{
    public void SCENE()
    {
        dataManager = FindAnyObjectByType<DataManager>();
    }
    public void LOADSCENE(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogWarning("Scene index out of range: " + index);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (collision.gameObject.CompareTag("Next"))
        {
            LOADSCENE(currentScene + 1);
        }

        if (collision.gameObject.CompareTag("Prev"))
        {
            LOADSCENE(currentScene -1);
        }
    }

}
