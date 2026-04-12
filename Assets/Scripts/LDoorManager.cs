using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Player _player;
    private bool _playerOverlapping = false;

    private void Start()
    {
        _player = Player.Instance;
    }

    private void Update()
    {
        if (_playerOverlapping && Mouse.current.leftButton.isPressed)
        {
            Debug.Log("clicked");
            TryWin();
        }
    }

    private void TryWin()
    {
        if (_player == null) return;

       
        if (!_player.u_ovalOfficeUnlocked)
        {
            Debug.Log("Door is locked. Buy the key from shop.");
            return;
        }

        WinGame();
    }

    private void WinGame()
    {
        Debug.Log("YOU WIN ");

        Time.timeScale = 0f;

        
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
