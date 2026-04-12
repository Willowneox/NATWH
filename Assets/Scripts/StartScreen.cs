using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject sceneManager;
    public GameObject cutsceneCanvas;

    public void StartCutscene()
    {
        sceneManager.SetActive(true);
        cutsceneCanvas.SetActive(true);

        gameObject.SetActive(false);
    }
}
