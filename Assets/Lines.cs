using UnityEngine;
using System.Collections;
using TMPro;

public class Lines : MonoBehaviour
{
    public TextMeshProUGUI textBox; // or TextMeshPro for world-space
    
    public string[] lines = {
        "Wow look at you go, everything is so clean!",
        "There is no better cleaning robot than you.",
        "Great work Peter! Keep working hard!",
        "Keeping cleaning Peter, everything is so spotless thanks to you."
    };

    public void OnEnable()
    {
        textBox.text = lines[Random.Range(0, lines.Length)];
        StartCoroutine(CloseSpeechAfterSeconds(5f));
    }

    private IEnumerator CloseSpeechAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            textBox.gameObject.SetActive(false);
        }
}
