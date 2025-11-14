using UnityEngine;

public class Pig : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("The initial and maximum health of the pig.")]
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("The minimum collision velocity required to register damage.")]
    [SerializeField] private float minImpactForceToDamage = 1.0f;

    [Tooltip("The multiplier to convert impact velocity into damage points.")]
    [SerializeField] private float damageMultiplier = 10f;

    [Header("Audio Settings")]
    [Tooltip("The sound clip to play when the pig is destroyed.")]
    [SerializeField] private AudioClip deathSoundClip;

    private float currentHealth;
    private bool isDead = false;
    private AudioSource audioSource; 

    // Called when the script instance is being loaded (before Start)
    void Awake()
    {
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // We only want it to play when Die() is called
        }
    }

    // Called when another collider touches this pig's collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        float impactForce = collision.relativeVelocity.magnitude;

        if (impactForce > minImpactForceToDamage)
        {
            float damage = impactForce * damageMultiplier;

            TakeDamage(damage);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (audioSource != null && deathSoundClip != null)
        {
            audioSource.PlayOneShot(deathSoundClip);
        }

        //Remove the object from the scene
        // We wait for the sound to finish before destroying the object.
        float destroyDelay = deathSoundClip != null ? deathSoundClip.length : 0f;
        Destroy(gameObject, destroyDelay);
    }
}