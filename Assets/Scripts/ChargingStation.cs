using UnityEngine;

public class ChargingStation : MonoBehaviour
{
   // public UpgradesCanvas shop;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           // shop.Openshop();
        }
    }
}
