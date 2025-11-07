using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Adjustable speed in the Inspector
    public float moveSpeed = 5f;

    // Smoothness of the rotation (higher number means faster rotation)
    public float rotationSpeed = 10f;

    private CharacterController controller;
    private Transform mainCameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform;

        if (controller == null)
        {
            Debug.LogError("PlayerController requires a CharacterController component!");
            enabled = false;
        }
        if (mainCameraTransform == null)
        {
            Debug.LogError("PlayerController could not find Main Camera!");
            enabled = false;
        }
    }

    void Update()
    {
        if (controller == null || mainCameraTransform == null) return;

        // 1. Get Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 2. Calculate World Movement Direction

        // Get the camera's forward and right vectors (ignoring Y for 2D movement)
        Vector3 cameraForward = mainCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = mainCameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        // Calculate the movement direction in World Space, relative to the camera view
        Vector3 moveDirection = (cameraForward * z) + (cameraRight * x);

        // 3. Apply Movement
        if (moveDirection.magnitude >= 0.1f)
        {
            // Move the character using the CharacterController
            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

            // 4. Handle Rotation (Character always faces the direction of movement)

            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // Smoothly rotate the player's root object
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}