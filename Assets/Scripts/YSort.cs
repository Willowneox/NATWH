using UnityEngine;

public class YSort : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _offset = 0f;

    private void LateUpdate()
    {
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y / 52.74f) + (int)_offset;
    }
}