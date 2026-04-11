using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clean : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Texture2D _dirtMaskBase;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private int _brushRadius = 20;

    public float percentCleaned => GetCleanedPercent();

    private Texture2D _templateDirtMask;

    private void Start()
    {
        CreateTexture();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Paint(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Paint(eventData);
    }

    private void Paint(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            eventData.position,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
            out Vector2 localPoint))
        {
            Rect rect = _rectTransform.rect;

            float normalizedX = (localPoint.x - rect.x) / rect.width;
            float normalizedY = (localPoint.y - rect.y) / rect.height;

            int pixelX = (int)(normalizedX * _templateDirtMask.width);
            int pixelY = (int)(normalizedY * _templateDirtMask.height);

            for (int x = -_brushRadius; x <= _brushRadius; x++)
            {
                for (int y = -_brushRadius; y <= _brushRadius; y++)
                {
                    if (x * x + y * y > _brushRadius * _brushRadius)
                        continue;

                    int targetX = pixelX + x;
                    int targetY = pixelY + y;

                    if (targetX < 0 || targetX >= _templateDirtMask.width ||
                        targetY < 0 || targetY >= _templateDirtMask.height)
                        continue;

                    _templateDirtMask.SetPixel(targetX, targetY, new Color(0, 0, 0, 0));
                }
            }

            _templateDirtMask.Apply();
            Debug.Log("Cleaned: " + percentCleaned.ToString("F1") + "%");

            if (percentCleaned >= 95)
            {
                // scrap reward
                Destroy(gameObject);
            }
        }
    }

    private void CreateTexture()
    {
        _templateDirtMask = new Texture2D(_dirtMaskBase.width, _dirtMaskBase.height);
        _templateDirtMask.SetPixels(_dirtMaskBase.GetPixels());
        _templateDirtMask.Apply();
        _rawImage.texture = _templateDirtMask;
    }
    private float GetCleanedPercent()
    {
        Color[] pixels = _templateDirtMask.GetPixels();
        int clearedCount = 0;

        foreach (Color pixel in pixels)
        {
            if (pixel.a == 0f)
                clearedCount++;
        }

        return (float)clearedCount / pixels.Length * 100f;
    }
}