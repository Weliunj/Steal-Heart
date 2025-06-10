using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Player : MonoBehaviour
{
    public void SCENE()
    {
        dataManager = FindAnyObjectByType<DataManager>();

        // Load scene đầu tiên nếu cần, ví dụ:
        // LOADSCENE(dataManager.prev);
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
            int nextScene = currentScene + 1;

            if (nextScene >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.Log("Đã ở scene cuối cùng!");
            }
            else
            {
                dataManager.prev = currentScene;
                dataManager.next = nextScene + 1;

                LOADSCENE(nextScene);
            }
        }

        if (collision.gameObject.CompareTag("Prev"))
        {
            int prevScene = currentScene - 1;

            if (prevScene < 0)
            {
                Debug.Log("Đã ở scene đầu tiên!");
            }
            else
            {
                dataManager.next = currentScene;
                dataManager.prev = prevScene - 1;

                LOADSCENE(prevScene);
            }
        }
    }

}
