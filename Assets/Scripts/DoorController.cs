using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject lockedVisual;
    public GameObject unlockedVisual;

    public bool IsUnlocked { get; private set; }

    Vector2Int _cellA;
    Vector2Int _cellB;

    public void Initialize(Vector2Int cellA, Vector2Int cellB)
    {
        _cellA = cellA;
        _cellB = cellB;
        IsUnlocked = DoorUnlockRegistry.IsUnlocked(cellA, cellB);
        RefreshVisuals();
    }

    // !!INCOMPLETE!!
    // player script needs:
    // - public variable Keys > tracks how many keys the player is holding
    // - public void UseKey() > Keys--
    public void Interact(GameObject player)
    {
        if (IsUnlocked) return;

        // if (player.Keys <= 0) return;

        // player.UseKey()
        Unlock();

        RoomManager.Instance.RefreshDoorBetween(_cellA, _cellB);
    }

    public void Unlock()
    {
        IsUnlocked = true;
        DoorUnlockRegistry.SetUnlocked(_cellA, _cellB);
        RefreshVisuals();
    }

    void RefreshVisuals()
    {
        lockedVisual?.SetActive(!IsUnlocked);
        unlockedVisual?.SetActive(IsUnlocked);
    }

}
