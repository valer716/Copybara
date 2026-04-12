using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float bulletSpeed = 10f;
    public float climbSpeed = 3f;
    public bool isGrounded = false;

    private float moveInput;
    private float verticalInput;

    private Rigidbody2D rb;
    private bool holdingSlingshot = false;

    private bool canClimbLeft = false;
    private bool canClimbRight = false;
    private bool isClimbing = false;

    [SerializeField] private Sprite armedCapybara;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D feetCollider;

    public float normalGravityScale = 1f;
    public float climbingGravityScale = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        HandleClimbing();
        HandleMovement();
        HandleJump();
        Flip();

        if (holdingSlingshot && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void HandleMovement()
    {
        if (!isClimbing)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Normál ugrás földrõl
            if (isGrounded && !isClimbing)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            // Ugrás mászható falról
            else if (isClimbing)
            {
                isClimbing = false;
                rb.gravityScale = normalGravityScale;

                float jumpDirection = 0f;

                if (canClimbLeft)
                {
                    jumpDirection = -1f; // bal oldali falról balra ugrik el
                }
                else if (canClimbRight)
                {
                    jumpDirection = 1f; // jobb oldali falról jobbra ugrik el
                }

                rb.linearVelocity = new Vector2(jumpDirection * moveSpeed, jumpForce);
            }
        }
    }

    private void HandleClimbing()
    {
        // Ha a fal bal oldala mászható, a játékos bal oldalon áll és JOBBRA nyomja magát neki
        bool pressingIntoLeftWall = canClimbLeft && moveInput > 0f;

        // Ha a fal jobb oldala mászható, a játékos jobb oldalon áll és BALRA nyomja magát neki
        bool pressingIntoRightWall = canClimbRight && moveInput < 0f;

        bool pressingIntoClimbableWall = pressingIntoLeftWall || pressingIntoRightWall;

        // Ha a megfelelõ oldalról nyomja a falat, elkezdhet mászni
        if (pressingIntoClimbableWall)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            // Ha már nem a fal felé nyomja magát, megszûnik a mászás
            if (!pressingIntoClimbableWall)
            {
                isClimbing = false;
                rb.gravityScale = normalGravityScale;
                return;
            }

            rb.gravityScale = climbingGravityScale;

            // Csak függõlegesen mozogjon mászás közben
            rb.linearVelocity = new Vector2(0f, verticalInput * climbSpeed);
        }
        else
        {
            rb.gravityScale = normalGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (other.otherCollider == feetCollider)
            {
                Vector2 normal = other.GetContact(0).normal;
                if (normal.y > 0.5f)
                {
                    isGrounded = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (other.otherCollider == feetCollider)
            {
                isGrounded = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ClimbableLeft"))
        {
            canClimbLeft = true;
        }

        if (other.CompareTag("ClimbableRight"))
        {
            canClimbRight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ClimbableLeft"))
        {
            canClimbLeft = false;
        }

        if (other.CompareTag("ClimbableRight"))
        {
            canClimbRight = false;
        }

        if (!canClimbLeft && !canClimbRight)
        {
            isClimbing = false;
        }
    }

    private void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    void OnEnable()
    {
        Slingshot.SlingshotPickedUp += HandleSlingshotPickedUp;
    }

    void OnDisable()
    {
        Slingshot.SlingshotPickedUp -= HandleSlingshotPickedUp;
    }

    void HandleSlingshotPickedUp()
    {
        spriteRenderer.sprite = armedCapybara;
        holdingSlingshot = true;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (isFacingRight)
        {
            bulletRb.linearVelocity = Vector2.right * bulletSpeed;
        }
        else
        {
            bulletRb.linearVelocity = Vector2.left * bulletSpeed;
        }

        Destroy(bullet, 5f);
    }
}