using UnityEngine;
using System.Collections;

public class EnemyHit : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    private bool isHit = false;
    private bool isDead = false;

    [Header("Hit Reaction)")]
    public float hitBackDistance = 0.6f;
    public float hitStunTime = 0.12f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isHit || isDead)
            return;

        if (HitStop.instance != null)
            HitStop.instance.Stop(0.06f);


        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitReaction());
        }
    }

    IEnumerator HitReaction()
    {
        isHit = true;

        // Flash
        sr.color = Color.gray;

        // Direction enemy is facing
        float dir = Mathf.Sign(transform.localScale.x);

        // Instant snap back (HK style)
        transform.position += new Vector3(-dir * hitBackDistance, 0f, 0f);

        yield return new WaitForSeconds(hitStunTime);

        sr.color = originalColor;
        isHit = false;
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
