using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenes2 : MonoBehaviour
{
    public Sprite[] frames;          // Drag your frames in the Inspector
    public float fps = 12f;          // Frames per second
    public Image displayImage;       // UI Image to display on

    private int currentFrame = 0;
    private float timer = 0f;

    void Update()
    {
        if (currentFrame > frames.Length - 1){
            return;
        }

        

        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            displayImage.sprite = frames[currentFrame];
            currentFrame++;
        }
        
    }
}
