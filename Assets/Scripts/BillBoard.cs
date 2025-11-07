using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No Main Camera found. Billboarding script disabled.");
            enabled = false;
        }
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        Vector3 cameraPosition = mainCamera.transform.position;

        // 1. Prevents the sprite from tilting up/down (locks rotation to the Y-axis)
        cameraPosition.y = transform.position.y;

        // 2. Rotates the sprite's forward direction to face the camera's position
        transform.LookAt(cameraPosition);

        // 3. Flips the sprite 180 degrees to correct SpriteRenderer's orientation
        transform.Rotate(0, 180, 0);
    }
}