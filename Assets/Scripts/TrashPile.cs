using UnityEngine;
using UnityEngine.Events;

public class TrashPile : MonoBehaviour
{
    public int TrashID { get; private set; }

    public UnityEvent<int> OnCleaned;

    bool _isCleaned = false;
    bool _inMinigame = false;

    public void Initialize(int id)
    {
        TrashID = id;
        _isCleaned = false;
        _inMinigame = false;
        gameObject.SetActive(true);
    }

    public void Interact()
    {
        if (_isCleaned || _inMinigame) return;
        _inMinigame = true;
        

        // RUN A MINIGAME FROM HERE (idk the implementation yet)
    }

    // Minigame should call this function
    // If game was won ->                     OnMinigameWin(true)
    // If game was lost or Ui was exited ->   OnMinigame(false)
    public void OnMinigameWin(bool success)
    {
        _inMinigame = false;

        if (success) Clean();
    }

    public void Clean()
    {
        _isCleaned = true;

        // vfx or animation of trash disappearing

        OnCleaned?.Invoke(TrashID);
        gameObject.SetActive(false);
    }
}
