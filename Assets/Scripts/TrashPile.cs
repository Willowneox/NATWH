using UnityEngine;
using UnityEngine.InputSystem;

public class TrashPile : MonoBehaviour
{
    [SerializeField] private Sprite _trashSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _minigamePrefab;
    [SerializeField] private AudioClip _cleanCompleteSound;
    [SerializeField] private float _cleanSoundVolume = 11f;

    public bool isOnTrigger { get; private set; } = false;
    public bool isCleaned = false;

    private void Awake()
    {
        _spriteRenderer.sprite = _trashSprite;
    }

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
        if (isOnTrigger && Mouse.current.leftButton.wasPressedThisFrame)
        {
            MinigameSpawner.Instance.OnMinigameComplete += OnMinigameComplete;
            MinigameSpawner.Instance.StartMinigame(_minigamePrefab);
        }
    }

    private void OnMinigameComplete()
    {
        isCleaned = true;
        Unsubscribe();

        if (_cleanCompleteSound != null)
            AudioSource.PlayClipAtPoint(_cleanCompleteSound, transform.position, _cleanSoundVolume);

        Destroy(gameObject);
    }

    private void Unsubscribe()
    {
        MinigameSpawner.Instance.OnMinigameComplete -= OnMinigameComplete;
    }
}