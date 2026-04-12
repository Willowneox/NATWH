using UnityEngine;

public class WinScript : MonoBehaviour
{

    private Player _player;
    public bool _playerOverlapping;
    public Canvas fadeoutCanvas;
    private void Start()
    {
        _player = Player.Instance;
    }
    private void OnMouseDown()
    {
        if (_playerOverlapping && _player.u_ovalOfficeUnlocked)
        {
            fadeoutCanvas.gameObject.SetActive(true);
            // load cutscene scene
        }
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
