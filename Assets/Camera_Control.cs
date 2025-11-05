using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    // --- Public Variables ---
    public Transform target;          // The player's Transform to orbit around
    public float lookSpeed = 3f;      // Mouse sensitivity for rotation
    public float distance = 5f;       // Fixed distance from the target

    // --- Private Variables for Camera State ---
    private float currentX = 0f;      // Current rotation angle on the X-axis (horizontal)
    private float currentY = 0f;      // Current rotation angle on the Y-axis (vertical)
    public float yMinLimit = -60f;    // Minimum vertical angle (look down limit)
    public float yMaxLimit = 60f;     // Maximum vertical angle (look up limit)

    // A reference to the camera's position we'll apply later
    private Vector3 offset;

    void Start()
    {
        // Calculate the initial rotation angles from the camera's starting position
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;

        // Ensure the camera is initially positioned correctly relative to the target
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    // Use LateUpdate for camera movement to ensure the player's movement is complete
    void LateUpdate()
    {
        if (target == null) return;

        // 1. Get Mouse Input
        // Multiply by Time.deltaTime to make rotation framerate independent (smoother)
        currentX += Input.GetAxis("Mouse X") * lookSpeed;
        currentY -= Input.GetAxis("Mouse Y") * lookSpeed;

        // 2. Clamp the Vertical Rotation
        // This stops the camera from flipping over the top/bottom of the player
        currentY = ClampAngle(currentY, yMinLimit, yMaxLimit);

        // 3. Calculate New Rotation and Position

        // Quaternion.Euler creates a rotation based on Euler angles (X, Y, Z)
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        // Calculate the new position: target position + (rotated offset vector)
        // Note: Vector3.back is a vector (0, 0, -1). 
        // Multiplying it by 'distance' gives a vector of length 'distance' pointing backwards.
        Vector3 position = target.position + rotation * (Vector3.back * distance);

        // 4. Apply to Camera Transform
        transform.rotation = rotation;
        transform.position = position;
    }

    // Helper function to keep angles within a range
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}