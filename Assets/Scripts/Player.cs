using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float playerAccel = 15f;
    public float speedCap = 8f;
    public float friction = 3f;

    [Header("Rigidbody2D")]
    public Rigidbody2D rb;

    [Header("Upgrades")]
    public int scrapCount = 0;
    int batteries = 0;

    private void Start()
    {
        rb.gravityScale = 0f;
    }
    void FixedUpdate()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Move();
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, friction * Time.fixedDeltaTime);
        }
    }

    private void Move()
    {
        // calculate movement direction
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        // accelerate
        rb.AddForce(direction * playerAccel, ForceMode2D.Force);

        // clamp to speed cap
        if (rb.linearVelocity.magnitude > speedCap)
            rb.linearVelocity = rb.linearVelocity.normalized * speedCap;
    }
}
