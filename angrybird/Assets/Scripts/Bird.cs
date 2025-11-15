using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Launch Settings")]
    public float launchForceMultiplier = 20f;
    public float maxPullDistance = 1.8f;

    [Header("Game Constraints")]
    public float minYBoundary = -1.5f;

    [Header("References")]
    public Transform slingshotAnchor;
    public Collider2D[] slingshotColliders;

    [HideInInspector]
    public bool isCurrentBird = false; // Is this the bird currently at the slingshot?

    private Rigidbody2D rb;
    private Collider2D birdCollider;
    private bool hasBeenLaunched = false;
    private bool nextBirdCalled = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        birdCollider = GetComponent<Collider2D>();
        rb.isKinematic = true;
    }

    void OnEnable()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        hasBeenLaunched = false;
        nextBirdCalled = false;

        if (isCurrentBird && slingshotAnchor != null)
            transform.position = slingshotAnchor.position;
    }

    private void OnMouseDown()
    {
        if (!isCurrentBird || hasBeenLaunched) return;

        rb.isKinematic = true;

        foreach (Collider2D col in slingshotColliders)
        {
            if (col != null)
                Physics2D.IgnoreCollision(birdCollider, col, true);
        }
    }

    private void OnMouseDrag()
    {
        if (!isCurrentBird || hasBeenLaunched) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector2 direction = (Vector2)mouseWorld - (Vector2)slingshotAnchor.position;

        if (direction.magnitude > maxPullDistance)
            direction = direction.normalized * maxPullDistance;

        Vector2 currentPos = (Vector2)slingshotAnchor.position + direction;

        if (currentPos.y < minYBoundary)
            currentPos.y = minYBoundary;

        transform.position = currentPos;
    }

    private void OnMouseUp()
    {
        if (!isCurrentBird || hasBeenLaunched) return;

        foreach (Collider2D col in slingshotColliders)
        {
            if (col != null)
                Physics2D.IgnoreCollision(birdCollider, col, false);
        }

        Vector2 launchVector = (Vector2)slingshotAnchor.position - (Vector2)transform.position;
        rb.isKinematic = false;
        hasBeenLaunched = true;

        rb.AddForce(launchVector * launchForceMultiplier, ForceMode2D.Impulse);
    }

    void Update()
    {
        // Trigger next bird only once
        if (isCurrentBird && hasBeenLaunched && !nextBirdCalled && rb.velocity.magnitude <= 0.1f)
        {
            nextBirdCalled = true;
            BirdManager.Instance.BirdFinished();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Offscreen or wall stops current bird
        if (isCurrentBird && collision.gameObject.CompareTag("OffscreenWall"))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            if (!nextBirdCalled)
            {
                nextBirdCalled = true;
                BirdManager.Instance.BirdFinished();
            }
        }
    }
}
