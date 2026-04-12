using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LinconDoor : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float interactDistance = 1f;

    private bool _playerOverlapping = false;
    private Player _player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = Player.Instance;
    }

    // Update is called once per frame
    private void OnMouseDown()
    {
       
        if (!_playerOverlapping) return;

        TryWin();
        Debug.Log("Yes, we are approaching to the Lincoln's");
    }


    private void TryWin()
    {
        if (_player == null) return;

        if (!_player.u_ovalOfficeUnlocked)
        {
            Debug.Log("Door is locked. Get the key from shop.");
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
      if(other.CompareTag("Player"))
            _playerOverlapping = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       if (other.CompareTag("Player"))
            _playerOverlapping = false;
    }

}
