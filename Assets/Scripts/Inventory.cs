using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    GameObject backpack;
    bool toggle = false;

    public void Start()
    {
        backpack = GetComponent<GameObject>();
        backpack.SetActive(toggle);
    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            backpack.SetActive(!toggle);
        }
    }
}
