using UnityEngine;

public class AlienTurret : MonoBehaviour
{
    [Header("Combat Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootInterval = 1.5f;
    public float detectionRange = 12f;
    public float verticalThreshold = -0.5f; 

    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;
    public GameObject explosionEffect;

    [Header("Visuals")]
    public Transform turretHead;
    public bool useSmoothRotation = true;
    public float rotationSpeed = 5f;

    private Transform player;
    private float shootTimer;

    void Start()
    {
        currentHealth = maxHealth;
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        float verticalDifference = player.position.y - transform.position.y;
        bool playerIsBelow = verticalDifference < verticalThreshold;

        if (distanceToPlayer <= detectionRange && !playerIsBelow)
        {
            TrackPlayer();

            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                Shoot();
                shootTimer = 0;
            }
        }
        else
        {
            shootTimer = 0;
        }
    }

    void TrackPlayer()
    {
        Vector2 direction = player.position - turretHead.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (useSmoothRotation)
            turretHead.rotation = Quaternion.Slerp(turretHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        else
            turretHead.rotation = targetRotation;
    }

    void Shoot()
    {
        GameObject projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projScript = projObj.GetComponent<Projectile>();

        if (projScript != null)
        {
            projScript.Launch(firePoint.right);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}