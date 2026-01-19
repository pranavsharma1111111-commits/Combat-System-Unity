using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    public bool canMove = true;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSprinting;

    private GravityDash dashScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashScript = GetComponent<GravityDash>(); 
    }

    void Update()
    {
        if (!canMove) return;

        if (dashScript != null && dashScript.isDashing) return;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            0.2f,
            groundLayer
        );

        float moveInput = Input.GetAxisRaw("Horizontal");
        isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}