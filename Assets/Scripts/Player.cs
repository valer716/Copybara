using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{   
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded = false;
    private float moveInput;
    private Rigidbody2D rb;


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
}
