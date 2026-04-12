using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        Debug.Log("You Win.");
        Time.timeScale = 1f;
        SceneManager.LoadScene("WinningScene");


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
