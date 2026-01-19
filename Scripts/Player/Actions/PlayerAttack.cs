using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Settings")]
    public GameObject attackBox;
    public float attackDuration = 0.15f;
    public float attackCooldown = 0.35f;

    [Header("Feel")]
    public float hitStopDuration = 0.06f;
    public float inputBufferTime = 0.2f;

    [Header("Recoil")]
    public float recoilForce = 5f; 

    private PlayerCombatStats combatStats;
    private PlayerDeflect playerDeflect;
    private DamageDealer damageDealer;
    private Rigidbody2D rb; 

    private Animator anim;

    private bool isAttacking = false;
    private float lastAttackTime;
    private float lastInputTime = -99f;

    void Start()
    {
        combatStats = GetComponent<PlayerCombatStats>();
        playerDeflect = GetComponent<PlayerDeflect>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (attackBox != null)
        {
            damageDealer = attackBox.GetComponent<DamageDealer>();
            attackBox.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            lastInputTime = Time.time;
        }

        if (isAttacking) return;
        if (playerDeflect != null && playerDeflect.IsDeflecting()) return;

        bool cooldownOver = Time.time >= lastAttackTime + attackCooldown;
        bool hasBufferedInput = Time.time - lastInputTime <= inputBufferTime;

        if (hasBufferedInput && cooldownOver)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        lastInputTime = -99f;

        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        if (damageDealer != null && combatStats != null)
            damageDealer.SetDamage(combatStats.attackDamage);

        attackBox.SetActive(true);
        CheckForHits();

        yield return new WaitForSeconds(attackDuration);

        attackBox.SetActive(false);
        lastAttackTime = Time.time;
        isAttacking = false;
    }

    void CheckForHits()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            attackBox.transform.position,
            attackBox.transform.lossyScale,
            attackBox.transform.eulerAngles.z
        );

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log("Hit Enemy: " + hit.name); 

                ApplyRecoil();

                StartCoroutine(HitStopRoutine());

                AlienTurret turret = hit.GetComponent<AlienTurret>();
                if (turret == null) turret = hit.GetComponentInParent<AlienTurret>();

                if (turret != null)
                {
                    turret.TakeDamage(combatStats.attackDamage);
                }
            }
        }
    }

    void ApplyRecoil()
    {
        if (rb != null)
        {
            float recoilDir = transform.localScale.x > 0 ? -1f : 1f;

            rb.velocity = new Vector2(0f, rb.velocity.y);

            rb.AddForce(new Vector2(recoilDir * recoilForce, 0f), ForceMode2D.Impulse);
        }
    }

    IEnumerator HitStopRoutine()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackBox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = attackBox.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}