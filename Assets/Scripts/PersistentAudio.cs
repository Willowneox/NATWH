using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public static GameMusic Instance;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
