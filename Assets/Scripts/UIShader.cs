using UnityEngine;
using UnityEngine.UI;

public class UIGlint : MonoBehaviour
{
    [SerializeField] private RawImage glintImage;
    [SerializeField] private float speed = 0.4f;

    private float _offset = -1f;

    void Update()
    {
        _offset += Time.deltaTime * speed;
        if (_offset > 2f) _offset = -1f;
        glintImage.uvRect = new Rect(_offset, 0f, 1f, 1f);
    }
}