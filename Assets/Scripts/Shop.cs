    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class Shop : MonoBehaviour
    {
        public Player player;
        public GameObject UpgradesCanvas;
        public Text upgradeStatusText;

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip shopOpenSound;
        public AudioClip shopCloseSound;
        public AudioClip purchaseSuccessSound;
        public AudioClip purchaseFailSound;

        [Header("Depth of Field")]
        public Volume postProcessVolume;
        private CanvasGroup upgradesCanvasGroup;
        private DepthOfField _dof;
        private LensDistortion _lensDistortion;
        public float focusDistanceBlurred = 0.1f;
        public float focusDistanceNormal = 2.82f;
        public float focusTransitionDuration = 1.0f;
        public float lensDistortionOpen = 0.5f;
        public float menuFadeDuration = 0.4f;
        public float lensDistortionClose = 0.35f;

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
        public int initBatteryCost = 0;
        public int initSpeedCost = 1;
        public int initMoneyCost = 6;
        public int initRoomKeyCost = 1;
        public int initVacuumCost = 8;
        public int ovalOfficeCost = 20;

        private void Awake()
        {
            if (postProcessVolume != null)
            {
                postProcessVolume.profile.TryGet(out _dof);
                postProcessVolume.profile.TryGet(out _lensDistortion);
            }
        }

        public void Start()
        {
            UpgradesCanvas = GameObject.Find("UpgradesCanvas");
            upgradesCanvasGroup = UpgradesCanvas.GetComponent<CanvasGroup>();
            if (upgradesCanvasGroup == null)
                upgradesCanvasGroup = UpgradesCanvas.AddComponent<CanvasGroup>();

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
            if (purchase(initRoomKeyCost))
            {
                player.keyCount++;
                player.keysPurchased++;
                roomKeyText.text = initRoomKeyCost.ToString();
            }
            UpdateUpgradeStatusUI();
        }

        public void BuyOvalOfficeKey()
        {
            if (player.u_ovalOfficeUnlocked) return;
            if (purchase(ovalOfficeCost))
            {
                player.u_ovalOfficeUnlocked = true;
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
                player.friction = 4;
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
            audioSource.PlayOneShot(shopOpenSound);
            StopAllCoroutines();
            StartCoroutine(OpenShopSequence());
        }

        public void CloseShop()
        {
            Player.Instance.UnfreezeMovement();
            Player.Instance.inShop = false;
            upgradesCanvasGroup.alpha = 1f;
            upgradesCanvasGroup.interactable = false;
            upgradesCanvasGroup.blocksRaycasts = false;
            UpgradesCanvas.SetActive(false);
            audioSource.PlayOneShot(shopCloseSound);
            StopAllCoroutines();
            if (_dof != null) StartCoroutine(LerpFocusDistance(focusDistanceNormal));
            if (_lensDistortion != null) StartCoroutine(LerpLensDistortion(lensDistortionClose));
        }

        private System.Collections.IEnumerator OpenShopSequence()
        {
            if (_dof != null) StartCoroutine(LerpFocusDistance(focusDistanceBlurred));
            if (_lensDistortion != null) StartCoroutine(LerpLensDistortion(lensDistortionOpen));
            yield return new WaitForSeconds(focusTransitionDuration);

            upgradesCanvasGroup.alpha = 0f;
            upgradesCanvasGroup.interactable = false;
            upgradesCanvasGroup.blocksRaycasts = false;
            UpgradesCanvas.SetActive(true);

            float elapsed = 0f;
            while (elapsed < menuFadeDuration)
            {
                elapsed += Time.deltaTime;
                upgradesCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / menuFadeDuration);
                yield return null;
            }
            upgradesCanvasGroup.alpha = 1f;
            upgradesCanvasGroup.interactable = true;
            upgradesCanvasGroup.blocksRaycasts = true;
        }

        private System.Collections.IEnumerator LerpFocusDistance(float target)
        {
            float start = _dof.focusDistance.value;
            float elapsed = 0f;
            while (elapsed < focusTransitionDuration)
            {
                elapsed += Time.deltaTime;
                _dof.focusDistance.value = Mathf.Lerp(start, target, elapsed / focusTransitionDuration);
                yield return null;
            }
            _dof.focusDistance.value = target;
        }

        private System.Collections.IEnumerator LerpLensDistortion(float target)
        {
            float start = _lensDistortion.intensity.value;
            float elapsed = 0f;
            while (elapsed < focusTransitionDuration)
            {
                elapsed += Time.deltaTime;
                _lensDistortion.intensity.value = Mathf.Lerp(start, target, elapsed / focusTransitionDuration);
                yield return null;
            }
            _lensDistortion.intensity.value = target;
        }

        private void UpdateUpgradeStatusUI()
        {
            string text = "";
            if (player.u_batteries > 0) text += "Battery: " + player.u_batteries + "\n";
            if (player.keysPurchased > 0) text += "Keys: " + player.keyCount + "\n";
            if (player.u_speed > 0) text += "Speed: " + player.u_speed + "\n";
            if (player.u_money > 0) text += "Efficiency Upgrades: " + player.u_money + "\n";
            if (player.u_vacuumFilterUnlocked) text += "Stickier Bristles Unlocked\n";
            if (player.u_ovalOfficeUnlocked) text += "Oval Office: Unlocked\n";
            upgradeStatusText.text = text;
        }

    private void Update()
    {
        UpdateUpgradeStatusUI();
    }
}