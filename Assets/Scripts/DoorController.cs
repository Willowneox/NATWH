using UnityEngine;
using UnityEngine.Events;
public class DoorController : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject lockedVisual;
    public GameObject unlockedVisual;

    public bool IsUnlocked { get; private set; }
    public bool isOnTrigger { get; private set; }

    private GameObject player1;

    Vector2Int _cellA;
    Vector2Int _cellB;

    public void Initialize(Vector2Int cellA, Vector2Int cellB)
    {
        _cellA = cellA;
        _cellB = cellB;
        IsUnlocked = DoorUnlockRegistry.IsUnlocked(cellA, cellB);
        RefreshVisuals();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            isOnTrigger = true;
            player1 = collision.gameObject;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isOnTrigger = false;
    }

    private void OnMouseDown(){
        if(isOnTrigger){
            Interact(player1);
        }
    }
    // !!INCOMPLETE!!
    // player script needs:
    // - public variable Keys > tracks how many keys the player is holding
    // - public void UseKey() > Keys--
    public void Interact(GameObject player)
    {
        if (IsUnlocked) return;

        if (player.GetComponent<Player>().u_roomCount <= 0) return;

        player.GetComponent<Player>().UseKey();
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
