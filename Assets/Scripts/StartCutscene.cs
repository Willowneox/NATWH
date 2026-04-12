using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class StartCutscene : MonoBehaviour
{
    public string sceneToLoad;
    public AnimationClip cutscene;
    public PlayableDirector director;
    public float changeTime;
    public int scene;
    private void Update()
    {

        changeTime -= Time.deltaTime;
        if (changeTime <= 0 && scene == 0)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void StartRoom1()
    {
        SceneManager.LoadScene(2);
    }
}
