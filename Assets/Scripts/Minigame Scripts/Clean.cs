using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clean : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Texture2D _dirtMaskBase;
    [SerializeField] private Texture2D _maskTexture;
    [SerializeField] private int _brushRadius = 20;

    public float percentCleaned => (float)_clearedPixels / _totalPixels * 100f;

    private Texture2D _templateDirtMask;
    private Canvas _canvas;
    private int _totalPixels;
    private int _clearedPixels;

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        CreateTexture();
    }

    public void OnPointerDown(PointerEventData eventData) { Paint(eventData); }
    public void OnDrag(PointerEventData eventData) { Paint(eventData); }

    private void Paint(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rectTransform,
            eventData.position,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
            out Vector2 localPoint)) return;

        Rect rect = _rectTransform.rect;
        int pixelX = (int)((localPoint.x - rect.x) / rect.width * _templateDirtMask.width);
        int pixelY = (int)((localPoint.y - rect.y) / rect.height * _templateDirtMask.height);

        int startX = Mathf.Clamp(pixelX - _brushRadius, 0, _templateDirtMask.width - 1);
        int startY = Mathf.Clamp(pixelY - _brushRadius, 0, _templateDirtMask.height - 1);
        int endX = Mathf.Clamp(pixelX + _brushRadius, 0, _templateDirtMask.width - 1);
        int endY = Mathf.Clamp(pixelY + _brushRadius, 0, _templateDirtMask.height - 1);

        int blockW = endX - startX + 1;
        int blockH = endY - startY + 1;

        Color[] block = _templateDirtMask.GetPixels(startX, startY, blockW, blockH);
        Color[] maskBlock = _maskTexture.GetPixels(startX, startY, blockW, blockH);

        for (int x = 0; x < blockW; x++)
        {
            for (int y = 0; y < blockH; y++)
            {
                int dx = (startX + x) - pixelX;
                int dy = (startY + y) - pixelY;
                if (dx * dx + dy * dy > _brushRadius * _brushRadius) continue;

                int idx = y * blockW + x;

                if (maskBlock[idx].a == 0f) continue;
                if (block[idx].a > 0f)
                {
                    block[idx] = new Color(0, 0, 0, 0);
                    _clearedPixels++;
                }
            }
        }

        _templateDirtMask.SetPixels(startX, startY, blockW, blockH, block);
        _templateDirtMask.Apply();

        Debug.Log("Cleaned: " + percentCleaned.ToString("F1") + "%");

        if (percentCleaned >= 98f)
        {
            MinigameSpawner.Instance.EndMinigame();
            Destroy(gameObject);
        }
    }

    private void CreateTexture()
    {
        _templateDirtMask = new Texture2D(_dirtMaskBase.width, _dirtMaskBase.height);
        _templateDirtMask.filterMode = FilterMode.Point;
        _templateDirtMask.SetPixels(_dirtMaskBase.GetPixels());
        _templateDirtMask.Apply();
        _rawImage.texture = _templateDirtMask;

        Color[] maskPixels = _maskTexture.GetPixels();
        Color[] dirtPixels = _dirtMaskBase.GetPixels();
        _totalPixels = 0;
        for (int i = 0; i < maskPixels.Length; i++)
            if (maskPixels[i].a > 0f && dirtPixels[i].a > 0f)
                _totalPixels++;

        Debug.Log("Total cleanable pixels: " + _totalPixels);
        _clearedPixels = 0;
    }
}