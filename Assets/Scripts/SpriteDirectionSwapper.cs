using UnityEngine;

public class SpriteDirectionSwapper : MonoBehaviour
{
    // Drag your directional sprites into these slots in the Inspector
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite rightSprite; // Only need ONE side sprite!

    public Transform playerRoot; // The root object with the CharacterController

    private SpriteRenderer spriteRenderer;
    private Transform mainCameraTransform;
    public float angleThreshold = 60f; // Controls how sensitive the swap is

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCameraTransform = mainCamera.transform;
        }

        if (playerRoot == null || spriteRenderer == null)
        {
            Debug.LogError("SpriteDirectionSwapper is missing required references (Player Root or Sprite Renderer).");
            enabled = false;
        }
    }

    void Update()
    {
        if (playerRoot == null || mainCameraTransform == null) return;

        // 1. Get the Raw Movement Input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(x, 0f, z).normalized;

        // Reset flip state every frame
        spriteRenderer.flipX = false;

        if (inputDirection.magnitude < 0.1f)
        {
            // If not moving, default to the front/idle sprite
            spriteRenderer.sprite = frontSprite;
            return;
        }

        // 2. Translate Input Direction to World Direction relative to the Camera
        Vector3 cameraForward = mainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDirectionWorld = (cameraForward * z + cameraRight * x).normalized;

        // 3. Determine Sprite based on Angle
        float angle = Vector3.SignedAngle(mainCameraTransform.forward, moveDirectionWorld, Vector3.up);

        if (angle < -angleThreshold || angle > angleThreshold)
        {
            // Angles near ±180: Player is moving towards the camera (S key/backward movement)
            // FIXED: Set to FRONT sprite
            spriteRenderer.sprite = frontSprite;
        }
        else if (angle < -angleThreshold / 2f)
        {
            // Angles near -90: Moving mostly LEFT (A key)
            spriteRenderer.sprite = rightSprite;
            spriteRenderer.flipX = true; // Flip the right sprite horizontally
        }
        else if (angle > angleThreshold / 2f)
        {
            // Angles near +90: Moving mostly RIGHT (D key)
            spriteRenderer.sprite = rightSprite;
            spriteRenderer.flipX = false; // Ensure it is not flipped
        }
        else
        {
            // Angle close to 0: Moving away from the camera (W key/forward movement)
            spriteRenderer.sprite = backSprite;
        }
    }
}