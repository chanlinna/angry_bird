using UnityEngine;

public class OffscreenWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object is a bird
        if (collision.gameObject.CompareTag("Bird"))
        {
            Bird birdScript = collision.gameObject.GetComponent<Bird>();
            if (birdScript != null && birdScript.isCurrentBird)
            {
                // Stop the bird
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }

                // Optionally destroy or deactivate the bird
                Destroy(collision.gameObject, 0.1f);

                // Notify BirdManager
                BirdManager.Instance.BirdFinished();
            }
        }
    }
}
