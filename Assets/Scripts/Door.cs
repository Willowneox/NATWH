using UnityEngine;
using UnityEngine.InputSystem;

public enum DoorDirection { North, South, East, West }

public class Door : MonoBehaviour
{
    [SerializeField] public DoorDirection direction;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _openSprite;
    [SerializeField] private Sprite _closedSprite;
    [SerializeField] private Collider2D _collider;

    public bool IsOpen { get; private set; } = false;
    public Room OwnerRoom { get; set; }

    private bool _playerOverlapping = false;
    private Player _player;

    private void Start()
    {
        _player = Player.Instance;
    }
    private void Update()
    {
        if (_playerOverlapping && !IsOpen && Mouse.current.leftButton.isPressed)
            TryOpen();
    }

    private void TryOpen()
    {
        if (_player == null || _player.u_roomCount <= 0) return;

        _player.u_roomCount--;
        RoomManager.Instance.OpenDoor(this);
    }

    public void Open()
    {
        IsOpen = true;
        _spriteRenderer.sprite = _openSprite;
        _collider.enabled = false;
    }

    public void Lock()
    {
        IsOpen = false;
        _spriteRenderer.sprite = _closedSprite;
        _collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _playerOverlapping = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _playerOverlapping = false;
    }
}