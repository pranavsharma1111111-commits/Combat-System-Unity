using UnityEngine;
using System.Collections;

public class EnemySimpleContact : MonoBehaviour
{
    public float detectionRange = 6f;
    public float attackRange = 1.1f;

    public float runSpeed = 3.5f;
    public float wanderSpeed = 1.2f;
    public float wanderSwitchTime = 2f;

    public int damage = 1;
    public float knockbackForceX = 7.5f;
    public float knockbackForceY = 4.2f;

    public float attackCooldown = 0.8f;
    public float pauseAfterHit = 0.35f;
    public float stunDuration = 0.5f;

    private Transform player;
    private Rigidbody2D rb;

    private float nextAttackTime;
    private bool isPaused;
    private bool isStunned;

    private int wanderDir = 1;
    private float wanderTimer;

    private Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        if (isPaused || isStunned)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= detectionRange)
        {
            ChasePlayer();
            TryAttack(dist);
        }
        else
        {
            Wander();
        }
    }

    void ChasePlayer()
    {
        int dir = player.position.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        rb.velocity = new Vector2(dir * runSpeed, rb.velocity.y);
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderSwitchTime)
        {
            wanderDir *= -1;
            wanderTimer = 0f;
        }

        transform.localScale = new Vector3(wanderDir, 1, 1);
        rb.velocity = new Vector2(wanderDir * wanderSpeed, rb.velocity.y);
    }

    void TryAttack(float dist)
    {
        if (Time.time < nextAttackTime) return;
        if (dist > attackRange) return;

        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph == null || !ph.CanBeHit()) return;

        nextAttackTime = Time.time + attackCooldown;

        rb.velocity = Vector2.zero;

        ph.TakeDamage(damage);

        int dir = player.position.x > transform.position.x ? 1 : -1;
        Vector2 force = new Vector2(dir * knockbackForceX, knockbackForceY);
        ph.ApplyKnockback(force);

        StartCoroutine(PauseEnemy());
    }

    IEnumerator PauseEnemy()
    {
        isPaused = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(pauseAfterHit);
        isPaused = false;
    }

    public void Stun()
    {
        if (!isStunned)
            StartCoroutine(StunRoutine());
    }

    IEnumerator StunRoutine()
    {
        isStunned = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public void ResetEnemy()
    {
        StopAllCoroutines();
        isPaused = false;
        isStunned = false;
        wanderTimer = 0f;
        wanderDir = 1;
        nextAttackTime = 0f;
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
    }
}
