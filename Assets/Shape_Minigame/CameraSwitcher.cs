using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // Store original 3D settings
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalFieldOfView;
    private Camera cameraComponent;
    private bool originalIsPerspective;

    [Header("Minigame 2D Settings")]
    // ⭐️ FIX: Camera positioned back on Z-axis, centered on X/Y plane (0, 0)
    public Vector3 minigame2DPosition = new Vector3(0, 0, -10);
    // ⭐️ FIX: Camera rotation set to look straight ahead (Front-On View)
    public Quaternion minigame2DRotation = Quaternion.Euler(0, 0, 0);
    // This value controls the zoom level in orthographic mode. Start with 5f.
    public float minigame2DSize = 5f;

    void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        if (cameraComponent == null)
        {
            Debug.LogError("CameraSwitcher must be attached to an object with a Camera component.");
            return;
        }

        SaveOriginalSettings();
    }

    public void SaveOriginalSettings()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalFieldOfView = cameraComponent.fieldOfView;
        originalIsPerspective = (cameraComponent.orthographic == false);
    }

    public void SwitchTo2DMinigame()
    {
        // Change to 2D Front-On View
        cameraComponent.orthographic = true;
        cameraComponent.orthographicSize = minigame2DSize;
        transform.position = minigame2DPosition;
        transform.rotation = minigame2DRotation;
    }

    public void SwitchTo3DMainGame()
    {
        // Restore 3D View
        cameraComponent.orthographic = !originalIsPerspective;
        if (originalIsPerspective)
        {
            cameraComponent.fieldOfView = originalFieldOfView;
        }
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}