using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteRandomizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image sprite;
    public List<Sprite> trashSprites;
    public RectTransform rt;

    private const float ScaleFactor = 2.5f;

    private Vector3 originalScale;
    public float enlargeFactor = 1.1f;
    public float duration = 0.1f;
    private Coroutine scaleCoroutine;
    private void Start()
    {
        sprite.sprite = trashSprites[Random.Range(0, trashSprites.Count)];
        originalScale = transform.localScale;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleTo(originalScale * enlargeFactor));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleTo(originalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}
