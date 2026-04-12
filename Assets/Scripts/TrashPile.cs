using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TrashPile : MonoBehaviour
{
    public bool isOnTrigger { get; private set; } = false;
    private bool isCleaned = false;

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
        Debug.Log(isOnTrigger + " " + Mouse.current.leftButton.wasPressedThisFrame);
        if (isOnTrigger && Mouse.current.leftButton.wasPressedThisFrame)
        {
            MinigameSpawner.Instance.OnMinigameComplete += OnMinigameComplete;
            MinigameSpawner.Instance.StartMinigame();
        }
    }

    private void OnMinigameComplete()
    {
        Debug.Log("Minigame complete");
        isCleaned = true;
        Unsubscribe();
        Destroy(gameObject);
    }
    private void Unsubscribe()
    {
        MinigameSpawner.Instance.OnMinigameComplete -= OnMinigameComplete;
    }
}