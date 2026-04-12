using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLifeUI : MonoBehaviour
{
    // ordered from full -> empty
    public List<Sprite> batterySprite = new List<Sprite>(7);
    public Image image;
    private Player p;

    private bool isRunning = false;
    void Start()
    {
        p = Player.Instance;
    }

    // 7 states

    private void Update()
    {
        if (p.batteryLeft >= p.batteryCapacity / 6 * 5)
        {
            image.sprite = batterySprite[0];
        }
        else if (p.batteryLeft >= p.batteryCapacity / 6 * 4)
        {
            image.sprite = batterySprite[1];
        }
        else if (p.batteryLeft >= p.batteryCapacity / 6 * 3)
        {
            image.sprite = batterySprite[2];
        }
        else if (p.batteryLeft >= p.batteryCapacity / 6 * 2)
        {
            image.sprite = batterySprite[3];
        }
        else if (p.batteryLeft >= p.batteryCapacity / 6)
        {
            image.sprite = batterySprite[4];
        }
        else
        {
            image.sprite = batterySprite[5];
        }
    }
}
