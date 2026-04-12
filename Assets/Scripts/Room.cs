using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door _doorNorth;
    [SerializeField] private Door _doorSouth;
    [SerializeField] private Door _doorEast;
    [SerializeField] private Door _doorWest;

    [SerializeField] private Transform _trashContainer;
    [SerializeField] private Collider2D _trashSpawnArea;
    [SerializeField] private GameObject _trashPrefab;
    [SerializeField] private GameObject _ObsPrefab;
    [SerializeField] private GameObject _PropPrefab;
    [SerializeField] private int _minTrash = 3;
    [SerializeField] private int _minObs = 3;
    [SerializeField] private int _maxObs = 8;
    [SerializeField] private int _minProp = 3;
    [SerializeField] private int _maxProp = 6;

    [SerializeField] private int _maxTrash = 9;
    [SerializeField] private float _trashCheckRadius = 0.5f;
    [SerializeField] private int _maxPlacementAttempts = 20;

    [SerializeField] private List<GameObject> _trashPrefabs;

    public bool IsGenerated { get; private set; } = false;

    private void Awake()
    {
        _doorNorth.OwnerRoom = this;
        _doorSouth.OwnerRoom = this;
        _doorEast.OwnerRoom = this;
        _doorWest.OwnerRoom = this;
    }

    public void Generate(DoorDirection? sharedDoorDirection = null)
    {
        if (IsGenerated) return;
        IsGenerated = true;

        SpawnTrash();
        SpawnObstacle();
        SpawnProps();

        if (sharedDoorDirection.HasValue)
            GetDoor(sharedDoorDirection.Value).Open();
    }

    public Door GetDoor(DoorDirection direction)
    {
        return direction switch
        {
            DoorDirection.North => _doorNorth,
            DoorDirection.South => _doorSouth,
            DoorDirection.East => _doorEast,
            DoorDirection.West => _doorWest,
            _ => null
        };
    }

    private void SpawnTrash()
    {
        int count = Random.Range(_minTrash, _maxTrash + 1);
        Bounds bounds = _trashSpawnArea.bounds;
        int layerMask = ~LayerMask.GetMask("TrashSpawnZone", "Ignore Raycast");
        Debug.Log($"Trying to spawn {count} trash. Bounds: {bounds.min} to {bounds.max}");

        for (int i = 0; i < count; i++)
        {
            bool spawned = false;
            for (int attempt = 0; attempt < _maxPlacementAttempts; attempt++)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y));

                bool inArea = _trashSpawnArea.OverlapPoint(randomPos);
                Collider2D hit = Physics2D.OverlapPoint(randomPos, layerMask);

                Debug.Log($"Attempt {attempt}: pos={randomPos} inArea={inArea} hit={hit?.gameObject.name ?? "none"} layer={hit?.gameObject.layer}");

                if (!inArea) continue;
                if (hit != null) continue;

                GameObject prefab = _trashPrefabs[Random.Range(0, _trashPrefabs.Count)];
                Instantiate(prefab, randomPos, Quaternion.identity, _trashContainer);
                spawned = true;
                Debug.Log($"Spawned trash at {randomPos}");
                break;
            }
            if (!spawned) Debug.Log($"Failed to spawn trash pile {i} after {_maxPlacementAttempts} attempts");
        }
    }

    private void SpawnObstacle()
    {
        int count = Random.Range(_minObs, _maxObs + 1);
        Bounds bounds = _trashSpawnArea.bounds;

        for (int i = 0; i < count; i++)
        {
            for (int attempt = 0; attempt < _maxPlacementAttempts; attempt++)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y));

                if (Physics2D.OverlapCircle(randomPos, _trashCheckRadius) != null)
                {
                    Instantiate(_ObsPrefab, randomPos, Quaternion.identity, _trashContainer);
                    break;
                }
            }
        }
    }

    private void SpawnProps()
    {
        int count = Random.Range(_minProp, _maxProp + 1);
        Bounds bounds = _trashSpawnArea.bounds;

        for (int i = 0; i < count; i++)
        {
            for (int attempt = 0; attempt < _maxPlacementAttempts; attempt++)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y));

                if (Physics2D.OverlapCircle(randomPos, _trashCheckRadius) != null)
                {
                
                    Instantiate(_PropPrefab, randomPos, Quaternion.identity, _trashContainer);
                    break;
                }
            }
        }
    }
    public void DisableDoor(DoorDirection direction)
    {
        Door door = GetDoor(direction);
        if (door == null) return;
        door.gameObject.SetActive(false);
    }
    
}