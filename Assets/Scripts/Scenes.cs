using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class Scenes : MonoBehaviour
{
    public Sprite[] frames;          // Drag your frames in the Inspector
    public float fps = 12f;          // Frames per second
    public Image displayImage;       // UI Image to display on

    public TMP_Text Text;
    private int currentFrame = 0;
    private float timer = 0f;

    public string[] line; 

    void Update()
    {
        if (currentFrame > frames.Length - 1){
            SceneManager.LoadScene("Scene1");
        }else{

        

        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            displayImage.sprite = frames[currentFrame];
            Text.text = line[currentFrame];
            currentFrame++;

        }
        }
    }
}
