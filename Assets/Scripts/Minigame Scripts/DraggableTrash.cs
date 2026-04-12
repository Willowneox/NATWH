using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableTrash : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rt;
    private Canvas canvas;
    private TrashDragSpawner spawner;
    [SerializeField] private Image sprite;

    public List<Sprite> trashSprites;

    private const float ScaleFactor = 2f;

    [SerializeField] DropZone dropZone;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
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
            spawner.PickupTrash();
            Destroy(gameObject);
        }
    }
}
