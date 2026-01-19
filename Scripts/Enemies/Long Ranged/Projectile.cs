using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public float maxDistance = 20f;
    public int damageAmount = 1;
    public LayerMask hitLayers;

    private Vector2 startPosition;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public void Launch(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        // 1. Check distance
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }

        // 2. Simple Collision
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f, hitLayers);
        if (hit != null)
        {
            if (hit.CompareTag("Player"))
            {
                var health = hit.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    // Calling the standard 1-parameter function
                    health.TakeDamage(damageAmount);
                }
            }

            Destroy(gameObject);
        }
    }
}