using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float dampTime = 0.15f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, dampTime);
    }
}
