using UnityEngine;

/// <summary>
/// Handles the destruction of a block when hit with sufficient force.
/// </summary>
public class DestructibleBlock : MonoBehaviour
{
    [Header("Impact Settings")]
    [Tooltip("The minimum force required to destroy the block instantly.")]
    public float requiredImpactForce = 50f;

    [Tooltip("The health of the block. Destruction happens if impact is too high or health reaches zero.")]
    public float health = 100f;

    // Note: We use OnCollisionEnter2D to detect impacts
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the relative speed/force of the collision
        float impactForce = collision.relativeVelocity.magnitude;

        // 1. Check for instant destruction from high force (like a solid hit from the Bird)
        if (impactForce > requiredImpactForce)
        {
            Debug.Log(gameObject.name + " destroyed instantly by high force: " + impactForce);
            DestroyBlock();
        }
        else
        {
            // 2. Apply damage based on the impact force
            health -= impactForce;

            // Optional: If you implement visual damage, change the sprite here.

            // 3. Check if health has dropped to zero
            if (health <= 0)
            {
                Debug.Log(gameObject.name + " destroyed after multiple hits. Final impact: " + impactForce);
                DestroyBlock();
            }
        }
    }

    /// <summary>
    /// Function to handle the actual destruction and potential score updates.
    /// </summary>
    private void DestroyBlock()
    {
        // TODO: In the next step (GameManager), add score here.
        // Example: GameManager.Instance.AddScore(50);

        // Simple destruction for the MVP
        Destroy(gameObject);
    }
}
