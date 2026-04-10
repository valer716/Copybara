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
    public bool isGrounded = false;
    private float moveInput;
    private Rigidbody2D rb;
    private bool holdingSlingshot = false;
    [SerializeField] private Sprite armedCapybara;
    [SerializeField] private GameObject bulletPrefab;


    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D feetCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
        
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);
        Flip();

        if (holdingSlingshot && Input.GetKeyDown(KeyCode.Mouse0)){
            Shoot();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if(other.otherCollider == feetCollider)
            {
                Vector3 normal = other.GetContact(0).normal;
                if (normal == Vector3.up)
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
            if(other.otherCollider == feetCollider)
            {
                isGrounded = false;
            }
            
        }
    }

    private void Flip(){
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f){
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

    private void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (isFacingRight){
            bulletRb.linearVelocity = transform.right * bulletSpeed;
        } else{
            bulletRb.linearVelocity = transform.right * bulletSpeed * -1;
        }
        
        Destroy(bullet, 5f);
    }
}
