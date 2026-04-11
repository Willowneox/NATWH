using UnityEngine;

public class ChargingStation : MonoBehaviour
{
    public Shop shop;

    private void OnMouseDown()
    {
        shop.OpenShop();
        Debug.Log("yes we are openeing the shop");
    }
}
