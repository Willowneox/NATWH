using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer sr;

    [Header("Movement Settings")]
    public float playerAccel = 15f;
    public const float INIT_SPEED_CAP = 8f;
    public float speedCap = INIT_SPEED_CAP;
    public float friction = 3f;
    public bool canMove = true;

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

    // Room key upgrade is 1 per time
    public int u_roomCount = 0; 
    public const float B_ROOM_COUNT = 1f;
    public const float U_ROOM_COUNT_PER_UPGRADE = 1f;

    // Oval office unlock is a 1 time purchase
    public bool u_ovalOfficeUnlocked = false;
    public bool u_vacuumFilterUnlocked = false;

    // Speed upgrade
    public int u_speed = 0;
    public const float B_SPEED = INIT_SPEED_CAP;
    public const float U_SPEED_PER_UPGRADE = 4f; // Might need to play with this number.

    // Scrap earning upgrade...
    public int u_money = 0;
    public const float B_SCRAP_EARNED = 1.0f;
    public const float U_SCRAP_EARNED_PER_UPGRADE = 0.2f; // Maybe consider using a growth function for these? idk

    private Vector2 lastMoveDirection = Vector2.down;

    public static Player Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

            rb.gravityScale = 0f;

        speedCap = B_SPEED;

        batteryLeft = B_BATTERY + u_batteries * U_BONUS_CHARGE_PER_BATTERY;
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (batteryLeft < 0)
        {
            triggerNoChargeEnding();
            return;
        }

        // decrement battery life
        batteryLeft -= Time.fixedDeltaTime;

        if (Mouse.current.leftButton.isPressed && canMove)
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

        // clamp to speed cap
        if (rb.linearVelocity.magnitude > speedCap)
            rb.linearVelocity = rb.linearVelocity.normalized * speedCap;

        // accelerate
        rb.AddForce(direction * playerAccel, ForceMode2D.Force);
    }

    // Called whenever an upgrade is purchased. Recalculates all stats. All 1 that is.
    public void handleUpgrade()
    {
        // battery life is taken care of in the start func, oval offic and vac are bools, scrap earned might depend on implementation of minigames
        // so this only touches speed for now.
        // Debug.Log("Handling speed upgrade.");
        speedCap = B_SPEED + u_speed * U_SPEED_PER_UPGRADE;
    }
    
    //animation
    private void UpdateAnimation()
    {
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        bool isFacingBack = lastMoveDirection.y > 0f;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFacingBack", isFacingBack);
        if (lastMoveDirection.x != 0)
        {
            sr.flipX = lastMoveDirection.x < 0;
        }
    }

    public void UseKey(){
        u_roomCount--;
    }
   
    public void FreezeMovement()
    {
        canMove = false;
        rb.linearVelocity = new Vector2(0, 0);
    }
    
    public void UnfreezeMovement()
    {
        canMove = true;
    }
    
     private void triggerNoChargeEnding()
    {
        FreezeMovement();
        // game over screen
    }
}

