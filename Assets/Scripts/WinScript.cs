using UnityEngine;
using UnityEngine.SceneManagement;
public class WinScript : MonoBehaviour
{

    private Player _player;
    public bool _playerOverlapping;
    public GameObject fadeoutCanvas;
    private void Start()
    {
        _player = Player.Instance;
    }

    public void Update()
    {
        if (_playerOverlapping && _player.u_ovalOfficeUnlocked)
        {
            fadeoutCanvas.SetActive(true);
            // load cutscene scene
            SceneManager.LoadScene("WinScene");
        }

    }

    private void OnMouseDown()
    {
        if (_playerOverlapping && _player.u_ovalOfficeUnlocked)
        {
            fadeoutCanvas.SetActive(true);
            // load cutscene scene
            SceneManager.LoadScene("WinScene");
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
