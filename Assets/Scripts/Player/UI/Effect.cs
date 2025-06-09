using UnityEngine;

public partial class UI : MonoBehaviour
{
    public GameObject Speed;
    public GameObject Jump;
    public GameObject Stre;
    public GameObject Burn;
    public GameObject Poison;
    public GameObject Slow;

    public void EFFECT()
    {
        Slow.SetActive(false);
        Burn.SetActive(false);
        Poison.SetActive(false);
        Speed.SetActive(false);
        Jump.SetActive(false);
        Stre.SetActive(false);
    }
}
