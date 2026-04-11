using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;

    [Header("Movement Settings")]
    public float playerAccel = 15f;
    public float speedCap = 8f;
    public float friction = 3f;

    [Header("Rigidbody2D")]
    public Rigidbody2D rb;

    [Header("Battery Life")]
    public float batteryLeft;

    [Header("Scrap Count")]
    public int scrap = 0;

    [Header("Upgrades")]
    // add u_ before these variables, to make the code easier to read

    // each upgrade needs a base value, number of upgrades, and benefit per upgrade
    // ex: battery has a base value of 20, u_batteries to track num of battery upgrades owned, and BONUS_CHARGE_PER_BATTERY
    public int u_batteries = 0;
    public const float B_BATTERY = 20f;
    public const float U_BONUS_CHARGE_PER_BATTERY = 5f;

    private Vector2 lastMoveDirection = Vector2.down;


    private void Start()
    {
        rb.gravityScale = 0f;

        batteryLeft = B_BATTERY + u_batteries * U_BONUS_CHARGE_PER_BATTERY;
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
        UpdateAnimation();
    }

    private void Move()
    {
        // calculate movement direction
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        //tell the direction
        if (direction.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = direction;
        }

        // accelerate
        rb.AddForce(direction * playerAccel, ForceMode2D.Force);

        // clamp to speed cap
        if (rb.linearVelocity.magnitude > speedCap)
            rb.linearVelocity = rb.linearVelocity.normalized * speedCap;
    }
    //animation
    private void UpdateAnimation()
    {
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        bool isFacingBack = lastMoveDirection.y > 0f;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFacingBack", isFacingBack);
    }
}
