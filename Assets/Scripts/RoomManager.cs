using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    // This script gets attached to game manager (or whatever we end up calling it)
    // Tracks player's current room and manages loading/deloading rooms
    public static RoomManager Instance { get; private set; }

    [Header("References")]
    public Transform playerTransform;
    public GameObject roomPrefab;

    [Header("Room Size (!!UPDATE!!)")]
    public float roomWidth = 20f;
    public float roomHeight = 20f;

    [Header("Load Radius")]
    [Header("Rooms within this distance are loaded")]
    public int loadRadius = 2;
    [Tooltip("Rooms beyond this distance are unloaded")]
    public int unloadRadius = 3;

    [Header("Pool Size")]
    public int poolSize = 30;

    // Runs when any room is done deloading
    public UnityEvent<Vector2Int> OnRoomFullyCleaned;

    readonly Dictionary<Vector2Int, RoomController> _activeRooms = new();
    readonly Dictionary<Vector2Int, RoomState> _visitedRooms = new();
    readonly Queue<RoomController> _pool = new();

    Vector2Int _lastPlayerCell = new Vector2Int(int.MaxValue, int.MaxValue);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        BuildPool();
        DoorUnlockRegistry.Reset();
        BuildPool();
    }

    private void Update()
    {
        Vector2Int currentCell = WorldToGrid(playerTransform.position);
        if (currentCell != _lastPlayerCell)
        {
            _lastPlayerCell = currentCell;
            RefreshActiveRooms(currentCell);
        }
    }

    public void RefreshDoorBetween(Vector2Int cellA, Vector2Int cellB)
    {
        if (_activeRooms.TryGetValue(cellA, out var roomA))
            roomA.RefreshDoor(cellB);
        if (_activeRooms.TryGetValue(cellB, out var roomB))
            roomB.RefreshDoor(cellA);
    }

    void BuildPool()
    {
        for (int i=0; i < poolSize; i++)
        {
            var go = Instantiate(roomPrefab, transform);
            go.SetActive(false);
            var controller = go.GetComponent<RoomController>();
            controller.OnRoomFullyCleaned.AddListener(cell => OnRoomFullyCleaned?.Invoke(cell));
            _pool.Enqueue(controller);
        }
    }
    RoomController RentFromPool()
    {
        if (_pool.Count > 0) return _pool.Dequeue();

        var go = Instantiate(roomPrefab, transform);
        var controller = go.GetComponent<RoomController>();
        controller.OnRoomFullyCleaned.AddListener(cell => OnRoomFullyCleaned?.Invoke(cell));
        return controller;
    }
    void ReturnToPool(RoomController room)
    {
        room.gameObject.SetActive(false);
        _pool.Enqueue(room);
    }
    void RefreshActiveRooms(Vector2Int center)
    {
        for (int x = -loadRadius; x <= loadRadius; x++)
        {
            for (int y = -loadRadius; y <= loadRadius; y++)
            {
                var cell = center + new Vector2Int(x, y);
                if (GridDistance(cell, center) <= loadRadius && !_activeRooms.ContainsKey(cell))
                    LoadRoom(cell);
            }
        }

        foreach (var cell in _activeRooms.Keys.ToList())
        {
            if (GridDistance(cell, center) > unloadRadius)
                UnloadRoom(cell);
        }
    }
    void LoadRoom(Vector2Int cell)
    {
        var state = GetOrCreateState(cell);
        var room = RentFromPool();

        room.transform.position = GridToWorld(cell);
        room.gameObject.SetActive(true);
        room.Configure(cell, state);

        _activeRooms[cell] = room;
    }

    void UnloadRoom(Vector2Int cell)
    {
        if (!_activeRooms.TryGetValue(cell, out var room)) return;
        ReturnToPool(room);
        _activeRooms.Remove(cell);
    }

    public void AdvanceDay()
    {
        WorldSeedManager.AdvanceDay();

        foreach (var cell in _activeRooms.Keys.ToList())
        {
            UnloadRoom(cell);
            LoadRoom(cell);
        }
    }
   
    RoomState GetOrCreateState(Vector2Int cell)
    {
        if(!_visitedRooms.TryGetValue(cell,out var state))
        {
            state = new RoomState();
            _visitedRooms[cell] = state;
        }
        return state;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x / roomWidth), Mathf.RoundToInt(worldPos.y / roomHeight));
    }
    public Vector3 GridToWorld(Vector2Int cell)
    {
        return new Vector3(cell.x * roomWidth, cell.y * roomHeight, 0f);
    }
    public static int GridDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }
}
