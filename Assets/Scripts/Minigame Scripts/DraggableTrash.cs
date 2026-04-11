using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableTrash : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rt;
    private Canvas canvas;
    private TrashDragSpawner spawner;

    [SerializeField] DropZone dropZone;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
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
