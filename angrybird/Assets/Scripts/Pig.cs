using UnityEngine;

public class Pig : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float minImpactForceToDamage = 1.0f;
    public float damageMultiplier = 10f;

    [Header("Scoring")]
    public int scoreValue = 1000; // Score awarded when this pig is destroyed

    [Header("Death Effect")]
    public GameObject deathEffectPrefab;

    private float currentHealth;
    private bool isDead = false;

    [Header("Audio Settings")]
    public AudioClip deathSoundClip;
    private AudioSource audioSource;

    void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // Use collision relative velocity to calculate damage
        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce > minImpactForceToDamage)
        {
            float damage = impactForce * damageMultiplier;

            TakeDamage(damage);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        // Optional: Visual feedback (shake, change color) could be added here

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Add Score Before Destruction 
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        // Play sound
        if (audioSource && deathSoundClip) audioSource.PlayOneShot(deathSoundClip);

        // Spawn death effect
        if (deathEffectPrefab)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        // Destroy after sound finishes
        float delay = deathSoundClip ? deathSoundClip.length : 0f;
        Destroy(gameObject, delay);
    }
    
}
