using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLifeUI : MonoBehaviour
{
    public List<Sprite> batterySprite = new List<Sprite>(7);
    public Image image;
    public float blinkSpeed = 3f;
    private Player p;

    void Start()
    {
        p = Player.Instance;
    }

    private void Update()
    {
        float chunkSize = p.batteryCapacity / 6f;
        float positionInChunk = p.batteryLeft % chunkSize;
        float percentInChunk = positionInChunk / chunkSize;
        bool shouldBlink = (percentInChunk < 0.5f && p.batteryLeft < chunkSize * 2);

        if (shouldBlink)
        {
            float alpha = Mathf.Max((Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f, 0.55f);
            image.color = new Color(1f, 1f, 1f, alpha);
        }
        else
        {
            image.color = Color.white;
        }

        if (p.batteryLeft >= chunkSize * 5) image.sprite = batterySprite[0];
        else if (p.batteryLeft >= chunkSize * 4) image.sprite = batterySprite[1];
        else if (p.batteryLeft >= chunkSize * 3) image.sprite = batterySprite[2];
        else if (p.batteryLeft >= chunkSize * 2) image.sprite = batterySprite[3];
        else if (p.batteryLeft >= chunkSize) image.sprite = batterySprite[4];
        else if (p.batteryLeft > 0) image.sprite = batterySprite[5];
        else image.color = new Color(1f, 1f, 1f, 0);
    }
}