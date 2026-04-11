
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RoomController : MonoBehaviour
{
    // This script is attached to every instance of a Room in the scene
    // A room is one "cell" in the world
    // Use Configure() to load this Room

    [Header("Door Controllers")]
    public DoorController doorNorth;
    public DoorController doorEast;
    public DoorController doorSouth;
    public DoorController doorWest;

    [Header("Locked Room Overlay")]
    public GameObject lockedOverlay; // set to black square in inspector

    [Header("Trash Spawnpoints")]
    public Transform[] trashSpawns;

    [Header("Trash Prefab")]
    public GameObject trashPilePrefab;

    [Header("# of Trash Spawns")]
    public int trashCount = 3;

    // we need to scale trash spawns per day using a global variable
    // should be in game manager
    // update this script when thats added

    public UnityEvent<Vector2Int> OnRoomFullyCleaned;

    public Vector2Int GridPosition { get; set; }

    RoomState _state;
    System.Random _layoutRng;
    System.Random _dailyRng;

    int _totalSpawnedToday = 0;
    readonly List<TrashPile> _activeTrash = new();


    public void Configure(Vector2Int gridPos, RoomState state)
    {
        GridPosition = gridPos;
        _state = state;
        _layoutRng = new System.Random(WorldSeedManager.GetLayoutSeed(gridPos.x, gridPos.y));
        _dailyRng = new System.Random(WorldSeedManager.GetDailySeed(gridPos.x, gridPos.y));

        _state.ValidateForDay(WorldSeedManager.CurrentDay);
        _activeTrash.Clear();
        _totalSpawnedToday = 0;

        SetupDoors();
        SpawnTrash();
    }
    void SetupDoors()
    {
        doorNorth?.gameObject.SetActive(HasDoorTo(GridPosition + Vector2Int.up));
        doorEast?.gameObject.SetActive(HasDoorTo(GridPosition + Vector2Int.right));
        doorSouth?.gameObject.SetActive(HasDoorTo(GridPosition + Vector2Int.down));
        doorWest?.gameObject.SetActive(HasDoorTo(GridPosition + Vector2Int.left));
    }

    void SetupDoor(DoorController door, Vector2Int neighbor)
    {
        if (door == null) return;

        bool hasDoor = HasDoorTo(neighbor);
        door.gameObject.SetActive(hasDoor);

        if (hasDoor) door.Initialize(GridPosition, neighbor);
    }
    bool HasDoorTo(Vector2Int neighbor)
    {
        int edgeSeed = WorldSeedManager.GetEdgeSeed(GridPosition, neighbor);
        return new System.Random(edgeSeed).NextDouble() > 0.4;
    }

    public void RefreshDoor(Vector2Int neighbor)
    {
        DoorController door = GetDoorToward(neighbor);
        if (door != null && !door.IsUnlocked)
            door.Unlock();
    }

    DoorController GetDoorToward(Vector2Int neighbor)
    {
        Vector2Int dir = neighbor - GridPosition;
        if (dir == Vector2Int.up) return doorNorth;
        if (dir == Vector2Int.down) return doorSouth;
        if (dir == Vector2Int.left) return doorWest;
        if (dir == Vector2Int.right) return doorEast;
        return null;
    }
    void SpawnTrash()
    {
        if (trashSpawns == null || trashSpawns.Length == 0) return;

        int count = Mathf.Min(trashCount, trashSpawns.Length);
        List<int> indices = ShuffledIndices(trashSpawns.Length, _dailyRng);

        for (int i = 0; i < count; i++)
        {
            int spawnIndex = indices[i];

            _totalSpawnedToday++;
            if (_state.IsTrashCleaned(spawnIndex)) continue;

            var go = Instantiate(trashPilePrefab, trashSpawns[spawnIndex].position, Quaternion.identity, transform);
            var pile = go.GetComponent<TrashPile>();

            if (pile != null)
            {
                int capturedId = spawnIndex;
                //pile.Initialize(capturedId);
                _activeTrash.Add(pile);
            }
        }
    }

    void OnTrashCleaned(int id)
    {
        _state.CleanTrash(id);
        if (_state.GetCleanPercent(_totalSpawnedToday) >= 1f)
            OnRoomFullyCleaned?.Invoke(GridPosition);
    }

    public float GetCleanPercent() => _state?.GetCleanPercent(_totalSpawnedToday) ?? 0f;

    static List<int> ShuffledIndices(int count, System.Random rng)
    {
        var indices = new List<int>(count);
        for (int i = 0; i < count; i++) indices.Add(i);

        for (int i = count - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        return indices;
    }
}
