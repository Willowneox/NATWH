using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public Player player;
    public GameObject UpgradesCanvas;
    public TextMeshProUGUI upgradeStatusText;


    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shopOpenSound;
    public AudioClip shopCloseSound;
    public AudioClip purchaseSuccessSound;
    public AudioClip purchaseFailSound;

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

    [Header("Initial Upgrade Costs")]
    // these are INPUTS to the fibonacci function
    public int initBatteryCost = 0;
    public int initSpeedCost = 2;
    public int initMoneyCost = 6;

    // these are RAW VALUES
    public int initRoomKeyCost = 1;
    public int initVacuumCost = 8;
    public int ovalOfficeCost = 20;
    
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
        UpdateUpgradeStatusUI();

        batteryText.text = initBatteryCost.ToString();
        roomKeyText.text = GrowthFunc.Fibonacci(initRoomKeyCost).ToString();
        speedText.text = GrowthFunc.Fibonacci(initSpeedCost).ToString();
        ovalOfficeKeyText.text = ovalOfficeCost.ToString();
        vacuumText.text = initVacuumCost.ToString();
        moneyText.text = GrowthFunc.Fibonacci(initMoneyCost).ToString();
    }

    public void BuyBattery()
    {
        if (purchase(initBatteryCost))
        {
            player.u_batteries++;
            batteryText.text = initBatteryCost.ToString();
        }
        UpdateUpgradeStatusUI();
    }
    
    public void BuyRoomKey()
    {
        int currCost = initRoomKeyCost;

        if (purchase(currCost))
        {
            player.keyCount++;
            player.keysPurchased++;
            roomKeyText.text = initRoomKeyCost.ToString();
        }
        UpdateUpgradeStatusUI();
    }

    // This ends the game. Not sure if you want it in the upgrade menu but if not then delete this ig
    public void BuyOvalOfficeKey()
    {
        if (player.u_ovalOfficeUnlocked) return; // already bought

        if (purchase(ovalOfficeCost))
        {
            player.u_ovalOfficeUnlocked = true;
            // Disable button

            
            // TODO: Either trigger end of game cutscene here OR grey out this upgrade and let the player
            // walk over to the specific door.
            ovalOfficeKeyText.text = "N/A";
            ovalOfficeButton.interactable = false;
            UpdateUpgradeStatusUI();
        }
    }

    public void BuySpeed()
    {

        int currCost = GrowthFunc.Fibonacci(player.u_speed + initSpeedCost);

        if (purchase(currCost))
        {
            player.scrap -= currCost;
            player.u_speed++;
            speedText.text = GrowthFunc.Fibonacci(player.u_speed + initSpeedCost).ToString();
            UpdateUpgradeStatusUI();
        }
    }

    public void BuyVacuumFilter()
    {
        if (player.u_vacuumFilterUnlocked) return;

        if (purchase(initVacuumCost))
        {
            player.u_vacuumFilterUnlocked = true;
            vacuumText.text = "N/A";
            vacuumButton.interactable = false;
            UpdateUpgradeStatusUI();
        }
        UpdateUpgradeStatusUI();

    }

    public void buyMoneyUpgrade()
    {
        int currCost = GrowthFunc.Fibonacci(player.u_money + initMoneyCost);
        if (purchase(currCost))
        {
            player.u_money++;
            initMoneyCost += 2;
            moneyText.text = GrowthFunc.Fibonacci(player.u_money + initMoneyCost).ToString();
        }
        UpdateUpgradeStatusUI();
    }

    public bool purchase(int cost)
    {
        if (player.scrap < cost)
        {
            audioSource.PlayOneShot(purchaseFailSound);
            return false;
        }

        player.scrap -= cost;
        player.handleUpgrade();
        audioSource.PlayOneShot(purchaseSuccessSound);
        return true;
    }

    public void OpenShop()
    {
        Player.Instance.FreezeMovement();
        Player.Instance.inShop = true;
        UpgradesCanvas.SetActive(true);
        audioSource.PlayOneShot(shopOpenSound);
    }

    public void CloseShop()
    {
        Player.Instance.UnfreezeMovement();
        Player.Instance.inShop = false;
        UpgradesCanvas.SetActive(false);
        audioSource.PlayOneShot(shopCloseSound);
    }

    private void UpdateUpgradeStatusUI()
    {
        string text = "";

        if (player.u_batteries > 0)
            text += "Battery: " + player.u_batteries + "\n";

        if (player.keysPurchased > 0)
            text += "Keys: " + player.keyCount + "\n";

        if (player.u_speed > 0)
            text += "Speed: " + player.u_speed + "\n";

        if (player.u_money > 0)
            text += "Efficiency Upgrades: " + player.u_money + "\n";

        if (player.u_vacuumFilterUnlocked)
            text += "Vacuum: Unlocked\n";

        if (player.u_ovalOfficeUnlocked)
            text += "Oval Office: Unlocked\n";

        if (text == "")
            text = "No upgrades yet";

        upgradeStatusText.text = text;
    }

}


