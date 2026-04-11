using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Player player;
    public GameObject UpgradesCanvas;

    // IDEA: Disable buttons you cannot afford

    [Header("Upgrade Cost Text")]
    public Text batteryText;
    public Text roomKeyText;
    public Text ovalOfficeKeyText;
    public Text speedText;
    public Text vacuumText;
    public Text moneyText;

    [Header("Buttons")]
    public Button vacuumButton;
    public Button ovalOfficeButton;

    [Header("Initial Upgrade Costs")] // these correspond to fibonacci numbers
    public int initBatteryCost = 3;
    public int initRoomKeyCost = 1;
    public int initSpeedCost = 3;
    public int initMoneyCost = 8;
    public int initVacuumCost = 500;
    public int ovalOfficeCost = 10000;
    
    /*
     *      For each upgrade, create a new public Text object for the cost
     *      and copy the structure of the BuyBattery() function
     *      
     *      Assign in editor:
     *      - Function to a new button
     *      - Upgrade cost text object its variable
     */
    public void Start()
    {
        UpgradesCanvas = GameObject.Find("UpgradesCanvas");
        UpgradesCanvas.SetActive(false);

        batteryText.text = GrowthFunc.Fibonacci(initBatteryCost).ToString();
        roomKeyText.text = GrowthFunc.Fibonacci(initRoomKeyCost).ToString();
        speedText.text = GrowthFunc.Fibonacci(initSpeedCost).ToString();
        ovalOfficeKeyText.text = ovalOfficeCost.ToString();
        vacuumText.text = initVacuumCost.ToString();
        moneyText.text = GrowthFunc.Fibonacci(initMoneyCost).ToString();
    }

    public void BuyBattery()
    {
        // calculate cost of next battery from growth function
        int currCost = GrowthFunc.Fibonacci(player.u_batteries + initBatteryCost);

        if (purchase(currCost))
        {
            player.u_batteries++;
            batteryText.text = GrowthFunc.Fibonacci(player.u_batteries + initBatteryCost).ToString();
        }
    }
    
    public void BuyRoomKey()
    {
        int currCost = GrowthFunc.Fibonacci(player.u_roomCount + initRoomKeyCost);

        if (purchase(currCost))
        {
            player.u_roomCount++;
            roomKeyText.text = GrowthFunc.Fibonacci(player.u_roomCount + initRoomKeyCost).ToString();
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
            // Disable button


            // TODO: Either trigger end of game cutscene here OR grey out this upgrade and let the player
            // walk over to the specific door.
            ovalOfficeKeyText.text = "N/A";
            ovalOfficeButton.interactable = false;
        }
    }

    public void BuySpeed()
    {
        int currCost = GrowthFunc.Fibonacci(player.u_speed + initSpeedCost);

        if (player.scrap < currCost) return;

        player.scrap -= currCost;
        player.u_speed++;
        speedText.text = GrowthFunc.Fibonacci(player.u_speed + initSpeedCost).ToString();
    }

    public void BuyVacuumFilter()
    {
        if (player.u_vacuumFilterUnlocked) return;

        const int cost = 500; // no growth function bc 1 time upgrade

        if (purchase(cost))
        {
            player.u_vacuumFilterUnlocked = true;
            vacuumText.text = "N/A";
            vacuumButton.interactable = false;
        }
    }

    public void buyMoneyUpgrade()
    {
        int currCost = GrowthFunc.Fibonacci(player.u_money + initMoneyCost);

        if (purchase(currCost))
        {
            player.u_money++;
            moneyText.text = GrowthFunc.Fibonacci(player.u_money + initMoneyCost).ToString();
        }
    }

    public bool purchase(int cost)
    {
               
        if (player.scrap < cost) return false;
        
        player.scrap -= cost;
        player.handleUpgrade();
        return true;
    }

    public void OpenShop()
    {
        Player.Instance.FreezeMovement();
        UpgradesCanvas.SetActive(true);
    }
        
        public void CloseShop()
    {
        Player.Instance.UnfreezeMovement();
        UpgradesCanvas.SetActive(false);
    }

}


