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
    [SerializeField] private int _minTrash = 3;
    [SerializeField] private int _minObs = 3;
    [SerializeField] private int _maxObs = 8;

    [SerializeField] private int _maxTrash = 9;
    [SerializeField] private float _trashCheckRadius = 1f;
    [SerializeField] private int _maxPlacementAttempts = 10;

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

        for (int i = 0; i < count; i++)
        {
            for (int attempt = 0; attempt < _maxPlacementAttempts; attempt++)
            {
                Vector2 randomPos = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y));

                if (Physics2D.OverlapCircle(randomPos, _trashCheckRadius) != null)
                {
                    Instantiate(_trashPrefab, randomPos, Quaternion.identity, _trashContainer);
                    break;
                }
            }
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
    public void DisableDoor(DoorDirection direction)
    {
        Door door = GetDoor(direction);
        if (door == null) return;
        door.gameObject.SetActive(false);
    }
    
}