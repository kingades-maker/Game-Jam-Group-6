using UnityEngine;

public class MovingDraggableShape : MonoBehaviour
{
    // FIX: Setting the default boundary to 25.6f here
    public float moveSpeed = 80f;
    public float boundaryZ = 25.6f; // <-- SET TO YOUR DESIRED VALUE

    private const float FixedYHeight = 0.5f;
    private const float PushAwayDistance = 2.0f;

    private Vector3 targetPosition;
    private Vector3 startPosition;

    private bool isDragging = false;

    private Vector3 offset;
    private float zCoord;
    public ShapeSpawner spawner;

    void Start()
    {
        // 1. Define the two Z-boundaries based on the public boundaryZ value
        Vector3 currentPos = transform.position;
        startPosition = new Vector3(currentPos.x, FixedYHeight, -boundaryZ);
        targetPosition = new Vector3(currentPos.x, FixedYHeight, boundaryZ);

        // 2. Start the shape at one end (e.g., -Z boundary)
        transform.position = startPosition;
    }

    void Update()
    {
        if (!isDragging)
        {
            // 1. Move toward the current target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // 2. Enforce fixed Y-height
            transform.position = new Vector3(transform.position.x, FixedYHeight, transform.position.z);

            // 3. Check if we reached the target
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Swap the target and start positions to reverse direction
                (startPosition, targetPosition) = (targetPosition, startPosition);
            }
        }
    }

    // --- PUSHBACK METHOD ---
    public void PushAway(Vector3 direction)
    {
        // Calculate the position we want to move the shape to (out of the box)
        Vector3 destination = transform.position + direction * PushAwayDistance;

        // Force the shape immediately to the destination
        transform.position = destination;

        // Recalculate movement targets to ensure the shape resumes its correct path
        Vector3 currentPos = transform.position;
        startPosition.x = currentPos.x;
        targetPosition.x = currentPos.x;
    }
    // --- END PUSHBACK METHOD ---

    // --- Dragging Logic ---

    void OnMouseDown()
    {
        isDragging = true;
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        Vector3 newPos = GetMouseWorldPos() + offset;
        // Keep the shape constrained to the fixed Y-height even when dragging
        transform.position = new Vector3(newPos.x, FixedYHeight, newPos.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnShapeDestroyed();
        }
    }
}
