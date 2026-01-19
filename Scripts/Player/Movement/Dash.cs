using System.Collections;
using UnityEngine;

public class GravityDash : MonoBehaviour
{
    public bool hasGravityShoes = true;
    [SerializeField] private float dashPower = 35f; 
    [SerializeField] private float dashTime = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    public Rigidbody2D rb;
    public TrailRenderer tr;

    [HideInInspector] public bool isDashing; 
    private bool canDash = true;

    void Update()
    {
        if (!hasGravityShoes) return;

        if (Input.GetKeyDown(KeyCode.Z) && canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float direction = transform.localScale.x > 0 ? 1 : -1;
        rb.velocity = new Vector2(direction * dashPower, 0f);

        if (tr != null) tr.emitting = true;

        yield return new WaitForSeconds(dashTime);

        if (tr != null) tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}