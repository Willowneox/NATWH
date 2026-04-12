using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrashTutorial : MonoBehaviour
{
    public TrashPile trashPile;
    public Text tutText1;
    public Text tutText2;

    private bool fading = false;
    private float duration = 2f;
    void Update()
    {
        if (trashPile.isCleaned && !fading)
        {
            fading = true;
        }
    }
}
