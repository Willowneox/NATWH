using UnityEngine;

public class DropZone : MonoBehaviour
{
    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public bool Contains(RectTransform other)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rt, RectTransformUtility.WorldToScreenPoint(null, other.position));
    }
}
