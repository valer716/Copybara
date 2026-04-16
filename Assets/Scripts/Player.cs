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
    public float timeAfterLastShot = 0f;
    [SerializeField] public float timeAfterLastShotDefault = 1.0f; //itt annyi a szám ahány másodpercenként lehessen újra lőni
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

    void FixedUpdate()
    {
        if (timeAfterLastShot > 0)
        {
            timeAfterLastShot -= Time.deltaTime;
        }

    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        HandleClimbing();
        HandleMovement();
        HandleJump();
        Flip();

        if (holdingSlingshot && timeAfterLastShot <=0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
            timeAfterLastShot = timeAfterLastShotDefault;
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
            // Normal ugras foldrol
            if (isGrounded && !isClimbing)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            // Ugr�s m�szhat� falr�l
            else if (isClimbing)
            {
                isClimbing = false;
                rb.gravityScale = normalGravityScale;

                float jumpDirection = 0f;

                if (canClimbLeft)
                {
                    jumpDirection = -1f; // bal oldali falr�l balra ugrik el
                }
                else if (canClimbRight)
                {
                    jumpDirection = 1f; // jobb oldali falr�l jobbra ugrik el
                }

                rb.linearVelocity = new Vector2(jumpDirection * moveSpeed, jumpForce);
            }
        }
    }

    private void HandleClimbing()
    {
        // Ha a fal bal oldala m�szhat�, a j�t�kos bal oldalon �ll �s JOBBRA nyomja mag�t neki
        bool pressingIntoLeftWall = canClimbLeft && moveInput > 0f;

        // Ha a fal jobb oldala m�szhat�, a j�t�kos jobb oldalon �ll �s BALRA nyomja mag�t neki
        bool pressingIntoRightWall = canClimbRight && moveInput < 0f;

        bool pressingIntoClimbableWall = pressingIntoLeftWall || pressingIntoRightWall;

        // Ha a megfelel� oldalr�l nyomja a falat, elkezdhet m�szni
        if (pressingIntoClimbableWall)
        {
            isClimbing = true;
        }

        if (isClimbing)
        {
            // Ha m�r nem a fal fel� nyomja mag�t, megsz�nik a m�sz�s
            if (!pressingIntoClimbableWall)
            {
                isClimbing = false;
                rb.gravityScale = normalGravityScale;
                return;
            }

            rb.gravityScale = climbingGravityScale;

            // Csak f�gg�legesen mozogjon m�sz�s k�zben
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
        GameObject bullet;
        if (isFacingRight){
            bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0, 0), transform.rotation);
        } else{
            bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0, 0), transform.rotation);
        }
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