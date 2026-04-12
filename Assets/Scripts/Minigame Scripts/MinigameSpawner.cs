using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigameSpawner : MonoBehaviour
{
    private GameObject currMinigame;
    private GameObject _pendingPrefab;
    public static MinigameSpawner Instance;
    public event Action OnMinigameComplete;

    private void Awake()
    {
        gameObject.SetActive(false);
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        currMinigame = Instantiate(_pendingPrefab, gameObject.transform);
        currMinigame.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        Destroy(currMinigame);
        currMinigame = null;
        _pendingPrefab = null;
    }

    public void StartMinigame(GameObject minigamePrefab)
    {
        _pendingPrefab = minigamePrefab;
        Player.Instance.FreezeMovement();
        gameObject.SetActive(true);
    }

    public void EndMinigame()
    {
        Player.Instance.scrap += (int)Mathf.Pow(Player.Instance.U_SCRAP_EARNED_PER_UPGRADE, Player.Instance.u_money);
        Player.Instance.UnfreezeMovement();
        OnMinigameComplete?.Invoke();
        gameObject.SetActive(false);
    }
}