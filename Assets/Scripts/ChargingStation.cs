using UnityEngine;
using UnityEngine.InputSystem;

public class ChargingStation : MonoBehaviour
{
    public Shop shop;

    public bool isOnTrigger { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isOnTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isOnTrigger = false;
    }
    private void OnMouseDown()
    {
        if (isOnTrigger && Mouse.current.leftButton.wasPressedThisFrame)
        {
            shop.OpenShop();
            Debug.Log("yes we are openeing the shop");
        }
    }
}
