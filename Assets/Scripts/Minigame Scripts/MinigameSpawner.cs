using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MinigameSpawner : MonoBehaviour
{
    public List<GameObject> minigamePrefabs;

    private GameObject currMinigame;

    public static MinigameSpawner Instance;

    private void Awake()
    {
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
        int index = Random.Range(0, minigamePrefabs.Count);

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
        gameObject.SetActive(false);
    }
}
