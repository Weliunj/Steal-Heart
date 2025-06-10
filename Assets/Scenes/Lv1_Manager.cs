using UnityEngine;
using UnityEngine.SceneManagement;

public class Lv1_Manager : MonoBehaviour
{
    [Header("References")]
    public DataManager dataManager;

    void Start()
    {
        ResetData();
    }

    void ResetData()
    {
        if (dataManager == null)
        {
            Debug.LogWarning("DataManager chưa được gán trong Lv1_Manager!");
            return;
        }

        // Reset item dữ liệu
        dataManager.Coin_Quan = 0;
        dataManager.Hp_bottle = 0;
        dataManager.Jump_bottle = 0;
        dataManager.Speed_bottle = 0;
        dataManager.Strength_bottle = 0;

        Debug.Log("DataManager đã được reset về mặc định.");
    }
}
