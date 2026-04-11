using System;
using System.Collections.Generic;
using UnityEngine;

public class MinigameSpawner : MonoBehaviour
{
    public List<GameObject> minigamePrefabs;

    private GameObject currMinigame;

    public static MinigameSpawner Instance;

    public event Action OnMinigameComplete;

    private void Awake()
    {
        gameObject.SetActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        int index = UnityEngine.Random.Range(0, minigamePrefabs.Count);

        currMinigame = minigamePrefabs[index];

        currMinigame = Instantiate(currMinigame, gameObject.transform);

        currMinigame.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        Destroy(currMinigame);
        currMinigame = null;
    }

    public void StartMinigame()
    {
        Player.Instance.FreezeMovement();
        gameObject.SetActive(true);
    }

    public void EndMinigame()
    {
        Player.Instance.UnfreezeMovement();
        OnMinigameComplete?.Invoke();
        gameObject.SetActive(false);
    }
}
