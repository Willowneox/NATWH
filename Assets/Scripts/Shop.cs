using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Player player;

    [Header("Upgrade Cost Text")]
    public Text button_text;

    /*
     *      For each upgrade, create a new public Text object for the cost
     *      and copy the structure of the BuyBattery() function
     *      
     *      Assign in editor:
     *      - Function to a new button
     *      - Upgrade cost text object its variable
     */
    public void BuyBattery()
    {
        // calculate cost of next battery from growth function
        int currCost = GrowthFunc.Fibonacci(player.u_batteries + 1);

        if (player.scrap < currCost) return;

        player.scrap -= currCost;
        player.u_batteries++;
        button_text.text = GrowthFunc.Fibonacci(player.u_batteries + 1).ToString();
    }
}
