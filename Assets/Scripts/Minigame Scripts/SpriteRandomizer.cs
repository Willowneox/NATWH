using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteRandomizer : MonoBehaviour
{
    public Image sprite;
    public List<Sprite> trashSprites;
    public RectTransform rt;

    private const float ScaleFactor = 2.5f;
    private void Start()
    {
        sprite.sprite = trashSprites[Random.Range(0, trashSprites.Count)];

        float aspectRatio = (float)sprite.sprite.texture.width / sprite.sprite.texture.height;
        float currentWidth = rt.rect.width * ScaleFactor;
        float currentHeight = rt.rect.height * ScaleFactor;

        float newWidth, newHeight;

        if (currentWidth / currentHeight > aspectRatio)
        {
            newHeight = currentHeight;
            newWidth = currentHeight * aspectRatio;
        }
        else
        {
            newWidth = currentWidth;
            newHeight = currentWidth / aspectRatio;
        }

        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
}
