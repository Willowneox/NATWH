using UnityEngine;
using UnityEngine.UI;

public class ScrapCounter : MonoBehaviour
{
    public Text text;
    private Player player;

    private void Start()
    {
        player = Player.Instance;
    }
    void Update()
    {
        text.text = player.scrap.ToString();
    }
}
