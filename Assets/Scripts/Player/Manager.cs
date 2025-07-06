using UnityEngine;

public partial class Player : MonoBehaviour
{
    BOD BOD;
    public void Ignore_Start()
    {
        Ignore("Item", "Enemy");
        Ignore("Shop", "Bullet");
        Ignore("Shop", "Enemy");
        //Bullet
        Ignore("Bullet", "Item");
        Ignore("Bullet", "Enemy");
        Ignore("Bullet", "BossRoom");
    }
    public void Ignore(string layer1, string layer2)
    {
        Physics2D.IgnoreLayerCollision
            (LayerMask.NameToLayer(layer1), 
            LayerMask.NameToLayer(layer2),
            true);
    }
}
