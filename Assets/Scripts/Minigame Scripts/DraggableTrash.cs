using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableTrash : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rt;
    private Canvas canvas;
    private TrashDragSpawner spawner;
    [SerializeField] private Image sprite;
    public List<Sprite> trashSprites;

    private const float ScaleFactor = 2f;

    [SerializeField] DropZone dropZone;
    private Vector3 originalScale;
    public float enlargeFactor = 1.1f;
    public float duration = 0.1f;
    private Coroutine scaleCoroutine;
    [SerializeField] private AudioClip[] _paperSounds = new AudioClip[3];
    [SerializeField] private float _paperSoundVolume = 1f;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
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

    public void Init(TrashDragSpawner spawner, DropZone dropZone)
    {
        this.spawner = spawner;
        this.dropZone = dropZone;
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropZone.Contains(rt))
        {
            PlayRandomSound();
            spawner.PickupTrash();
            Destroy(gameObject);
        }
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

    private void PlayRandomSound()
    {
        if (_paperSounds.Length == 0) return;
        AudioClip clip = _paperSounds[Random.Range(0, _paperSounds.Length)];
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, _paperSoundVolume);
    }
}