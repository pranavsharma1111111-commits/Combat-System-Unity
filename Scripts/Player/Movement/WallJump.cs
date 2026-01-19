using UnityEngine;

public class WallJump : MonoBehaviour
{
    [Header("Detection")]
    public float wallCheckDistance = 0.6f;
    public LayerMask wallLayer;
    public Transform groundCheck;

    [Header("Wall Slide")]
    public float slideSpeed = 2f;

    [Header("Jump Physics")]
    public float jumpForceUp = 12f;
    public float jumpForceAway = 10f;
    public float wallStunDuration = 0.2f;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private bool isGrounded;
    private int lastWallJumpedFrom = 0; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, wallLayer);
        bool wallRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        bool wallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        bool isTouchingWall = wallRight || wallLeft;

        if (isGrounded) lastWallJumpedFrom = 0;

        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
        }

        if (!isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (wallRight && lastWallJumpedFrom != 1)
            {
                ExecuteJump(Vector2.left, 1);
            }
            else if (wallLeft && lastWallJumpedFrom != 2)
            {
                ExecuteJump(Vector2.right, 2);
            }
        }
    }

    void ExecuteJump(Vector2 awayDir, int wallID)
    {
        lastWallJumpedFrom = wallID;
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine(awayDir));
    }

    System.Collections.IEnumerator WallJumpRoutine(Vector2 awayDir)
    {
        if (movement != null) movement.canMove = false;

        rb.velocity = new Vector2(awayDir.x * jumpForceAway, jumpForceUp);

        float newScaleX = awayDir.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(newScaleX, 1, 1);

        float timer = 0;
        while (timer < wallStunDuration)
        {
            timer += Time.deltaTime;
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f) break;
            yield return null;
        }

        if (movement != null) movement.canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.right * wallCheckDistance);
        Gizmos.DrawRay(transform.position, Vector2.left * wallCheckDistance);
    }
}