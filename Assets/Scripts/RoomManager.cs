using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private float _roomWidth = 20f;
    [SerializeField] private float _roomHeight = 20f;

    private Dictionary<Vector2Int, Room> _spawnedRooms = new();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        Room startRoom = FindAnyObjectByType<Room>();
        if (startRoom != null)
        {
            _spawnedRooms[Vector2Int.zero] = startRoom;
            startRoom.Generate();
        }
    }

    public void OpenDoor(Door door)
    {
        door.Open();

        Room ownerRoom = door.OwnerRoom;
        Vector2Int ownerCoord = GetCoord(ownerRoom);
        Vector2Int newCoord = ownerCoord + DirectionToOffset(door.direction);

        if (_spawnedRooms.TryGetValue(newCoord, out Room existingRoom))
        {
            existingRoom.GetDoor(Opposite(door.direction)).Open();
            return;
        }

        Vector3 newPos = ownerRoom.transform.position + DirectionToWorld(door.direction);
        Room newRoom = Instantiate(_roomPrefab, newPos, Quaternion.identity)
            .GetComponent<Room>();

        _spawnedRooms[newCoord] = newRoom;
        newRoom.Generate(Opposite(door.direction));
    }

    private Vector2Int GetCoord(Room room)
    {
        foreach (var kvp in _spawnedRooms)
            if (kvp.Value == room) return kvp.Key;
        return Vector2Int.zero;
    }

    private Vector2Int DirectionToOffset(DoorDirection dir)
    {
        return dir switch
        {
            DoorDirection.North => Vector2Int.up,
            DoorDirection.South => Vector2Int.down,
            DoorDirection.East => Vector2Int.right,
            DoorDirection.West => Vector2Int.left,
            _ => Vector2Int.zero
        };
    }

    private Vector3 DirectionToWorld(DoorDirection dir)
    {
        return dir switch
        {
            DoorDirection.North => new Vector3(0, _roomHeight, 0),
            DoorDirection.South => new Vector3(0, -_roomHeight, 0),
            DoorDirection.East => new Vector3(_roomWidth, 0, 0),
            DoorDirection.West => new Vector3(-_roomWidth, 0, 0),
            _ => Vector3.zero
        };
    }

    private DoorDirection Opposite(DoorDirection dir)
    {
        return dir switch
        {
            DoorDirection.North => DoorDirection.South,
            DoorDirection.South => DoorDirection.North,
            DoorDirection.East => DoorDirection.West,
            DoorDirection.West => DoorDirection.East,
            _ => dir
        };
    }
}