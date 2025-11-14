using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    [Header("Launch Settings")]
    [Tooltip("Multiplier for the force applied during launch.")]
    public float launchForceMultiplier = 200f;

    [Tooltip("The maximum distance the bird can be pulled from its starting position.")]
    public float maxPullDistance = 1.8f;

    [Header("Game Constraints")]
    [Tooltip("The lowest Y-coordinate the bird can be dragged to (the top of the ground collider).")]
    public float minYBoundary = -1.5f;

    [Header("References")]
    [Tooltip("The static point the slingshot pulls from.")]
    public Transform slingshotAnchor;

    [Tooltip("The colliders of the slingshot posts, to be ignored during drag.")]
    public Collider2D[] slingshotColliders;

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool hasBeenLaunched = false;
    private Collider2D birdCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        birdCollider = GetComponent<Collider2D>();

        if (slingshotAnchor != null)
        {
            initialPosition = slingshotAnchor.position;
            transform.position = initialPosition;
        }
        else
        {
            Debug.LogError("SlingshotAnchor is not assigned! Bird won't work correctly.");
            initialPosition = transform.position;
        }

        rb.isKinematic = true;
    }

    private void OnMouseDown()
    {
        if (hasBeenLaunched) return;

        rb.isKinematic = true;

        foreach (Collider2D collider in slingshotColliders)
        {
            if (collider != null)
            {
                Physics2D.IgnoreCollision(birdCollider, collider, true);
            }
        }
    }


    private void OnMouseDrag()
    {
        if (hasBeenLaunched) return;

        // Convert mouse position to world coordinates.
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPoint.z = 0;

        // Calculate direction and clamp to max pull distance.
        Vector2 currentPosition = mouseWorldPoint;
        Vector2 direction = currentPosition - initialPosition;

        if (direction.magnitude > maxPullDistance)
        {
            direction = direction.normalized * maxPullDistance;
            currentPosition = initialPosition + direction;
        }

        // APPLY GROUND CLAMPING LOGIC
        if (currentPosition.y < minYBoundary)
        {
            currentPosition.y = minYBoundary;
        }

        transform.position = currentPosition;
    }

    private void OnMouseUp()
    {
        if (hasBeenLaunched) return;


        foreach (Collider2D collider in slingshotColliders)
        {
            if (collider != null)
            {
                Physics2D.IgnoreCollision(birdCollider, collider, false);
            }
        }

        // Calculate the launch vector (opposite of the drag direction).
        Vector2 launchDirection = initialPosition - (Vector2)transform.position;

        // Launch the bird.
        rb.isKinematic = false;
        hasBeenLaunched = true;

        rb.AddForce(launchDirection * launchForceMultiplier, ForceMode2D.Impulse);

    }

    private void CheckStop()
    {
        
    }
}