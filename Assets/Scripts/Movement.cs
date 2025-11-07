using UnityEngine;
public class Movement : MonoBehaviour
{
    public Mesh forwardMesh;
    public Mesh backMesh;
    public Mesh sideMesh;
    public Transform modelTransform; // Drag "Model" here
    public Transform visualTransform; // NEW - Drag "Visual" here (the one with MeshFilter)
    public float speed;
    Rigidbody rb;
    public Transform cameraTransform; // Assign this in the inspector

    void Start()
    {
        // Get MeshFilter from Visual, not Model
        meshFilter = visualTransform.GetComponent<MeshFilter>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float h_input = Input.GetAxis("Horizontal");
        float v_input = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * v_input + right * h_input) * speed;
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }
}