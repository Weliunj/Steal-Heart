using UnityEngine;

public partial class Player : MonoBehaviour
{
    public void IgnoreObj()
    {
        //Coin
        Ignore("Coin", "Enemy");
        Ignore("Coin", "Bullet");
    }
    public void Ignore(string layer1, string layer2)
    {
        Physics2D.IgnoreLayerCollision
            (LayerMask.NameToLayer(layer1), 
            LayerMask.NameToLayer(layer2),
            true);
    }
}
