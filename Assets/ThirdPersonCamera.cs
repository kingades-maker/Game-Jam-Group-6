using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObject;
    public Rigidbody rb;

    public float rotationSpeed;

    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(inputDirection != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }

    }


}
