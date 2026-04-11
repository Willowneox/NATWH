using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Player player;

    [Header("Upgrade Cost Text")]
    public Text batteryText;
    public Text roomKeyText;
    public Text ovalOfficeKeyText;
    public Text speedText;
    public Text vacuumText;
    public Text moneyText;

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

        if (purchase(currCost))
        {
            player.u_batteries++;
            batteryText.text = GrowthFunc.Fibonacci(player.u_batteries + 1).ToString();
        }
    }

    public void BuyRoomKey()
    {
        int currCost = GrowthFunc.Fibonacci(player.u_roomCount + 1);

        if (purchase(currCost))
        {
            player.u_roomCount++;
            roomKeyText.text = GrowthFunc.Fibonacci(player.u_roomCount + 1).ToString();
        }
    }

    // This ends the game. Not sure if you want it in the upgrade menu but if not then delete this ig
    public void BuyOvalOfficeKey()
    {
        if (player.u_ovalOfficeUnlocked) return; // already bought

        const int cost = 10000; // no growth function bc 1 time upgrade

        if (purchase(cost))
        {
            player.u_ovalOfficeUnlocked = true;
            // TODO: Either trigger end of game cutscene here OR grey out this upgrade and let the player
            // walk over to the specific door.
            ovalOfficeKeyText.text = "N/A";
        }
    }

    public void BuySpeed()
    {
        int currCost = GrowthFunc.Fibonacci(player.u_speed + 1);

        if (player.scrap < currCost) return;

        player.scrap -= currCost;
        player.u_speed++;
        speedText.text = GrowthFunc.Fibonacci(player.u_speed + 1).ToString();
    }

    public void BuyVacuumFilter()
    {
        if (player.u_vacuumFilterUnlocked) return;

        const int cost = 500; // no growth function bc 1 time upgrade

        if (purchase(cost))
        {
            player.u_vacuumFilterUnlocked = true;
            vacuumText.text = "N/A";
        }
    }

    public void buyMoneyUpgrade()
    {
        const int offset = 10; // make this start at a higher cost for **balance** reasons
        int currCost = GrowthFunc.Fibonacci(player.u_money + offset);

        if (purchase(currCost))
        {
            player.u_money++;
            moneyText.text = GrowthFunc.Fibonacci(player.u_money + offset).ToString();
        }
    }

    public bool purchase(int cost)
    {
               
        if (player.scrap < cost) return false;
        
        player.scrap -= cost;
        player.handleUpgrade();
        return true;
    }
}
