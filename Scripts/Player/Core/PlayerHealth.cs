using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;

    [Header("Invincibility Frames")]
    public float invincibleTime = 0.45f;
    public float flashInterval = 0.08f;

    [Header("UI")]
    public Slider hpBar;
    public GameObject gameOverPanel;

    private int currentHealth;
    private bool isDead = false;
    private bool isInvincible = false;

    private SpriteRenderer sr;
    private Color originalColor;

    private PlayerMovement movement;
    private PlayerAttack attack;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();

        if (sr != null)
            originalColor = sr.color;
    }

    void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        if (!CanBeHit())
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateUI();

        StartCoroutine(InvincibilityRoutine());

        if (currentHealth <= 0)
            Die();
    }

    public bool CanBeHit()
    {
        return !isDead && !isInvincible;
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float elapsed = 0f;
        bool visible = true;

        while (elapsed < invincibleTime)
        {
            if (sr != null)
                sr.color = visible ? Color.red : originalColor;

            visible = !visible;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        if (sr != null)
            sr.color = originalColor;

        isInvincible = false;
    }

    public void ApplyKnockback(Vector2 force)
    {
        StartCoroutine(KnockbackRoutine(force));
    }

    IEnumerator KnockbackRoutine(Vector2 force)
    {
        if (movement != null)
            movement.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.15f);

        if (movement != null)
            movement.enabled = true;
    }


    void Die()
    {
        isDead = true;

        Time.timeScale = 0f;

        if (movement != null)
            movement.enabled = false;

        if (attack != null)
            attack.enabled = false;

        if (hpBar != null)
            hpBar.gameObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }


    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        isInvincible = false;

        UpdateUI();

        if (movement != null)
            movement.enabled = true;

        if (attack != null)
            attack.enabled = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void UpdateUI()
    {
        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
            hpBar.value = currentHealth;
            hpBar.gameObject.SetActive(true);
        }
    }
}