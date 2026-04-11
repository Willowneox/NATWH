using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float playerAccel = 15f;
    public const float INIT_SPEED_CAP = 8f;
    public float speedCap = INIT_SPEED_CAP;
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

    // Start with 1 room, each key opens 1 more. Am I doing this right??
    public int u_roomCount = 0;
    public const float B_ROOM_COUNT = 1;
    public const float U_ROOMS_PER_UPGRADE = 1;

    // Oval office unlock is a 1 time purchase
    public bool ovalOfficeUnlocked = false;

    // Speed upgrade
    public int u_speed = 0;
    public const float B_SPEED = INIT_SPEED_CAP;
    public const float U_SPEED_PER_UPGRADE = 4f; // Might need to play with this number.

    // Scrap earning upgrade...

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
