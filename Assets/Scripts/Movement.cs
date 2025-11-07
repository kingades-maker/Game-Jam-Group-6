using UnityEngine;
public class Movement : MonoBehaviour
{
    public Mesh forwardMesh;
    public Mesh backMesh;
    public Mesh sideMesh;
    public Transform modelTransform; // Drag "Model" here
    public Transform visualTransform; // NEW - Drag "Visual" here (the one with MeshFilter)
    public float speed;

    private MeshFilter meshFilter;
    private Rigidbody rb;

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
        float h_input = Input.GetAxis("Horizontal") * speed;
        float v_input = Input.GetAxis("Vertical") * speed;
        rb.linearVelocity = new Vector3(v_input, rb.linearVelocity.y, -h_input);

        if (Mathf.Abs(h_input) > Mathf.Abs(v_input))
        {
            if (sideMesh != null)
            {
                meshFilter.mesh = sideMesh;

                if (h_input < 0) // Moving left
                {
                    visualTransform.localRotation = Quaternion.Euler(0, 180, 0);
                }
                else // Moving right
                {
                    visualTransform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
        else if (Mathf.Abs(v_input) > 0.01f)
        {
            visualTransform.localRotation = Quaternion.Euler(0, 0, 0);

            if (v_input > 0 && backMesh != null)
            {
                meshFilter.mesh = backMesh;
            }
            else if (v_input < 0 && forwardMesh != null)
            {
                meshFilter.mesh = forwardMesh;
            }
        }
    }
}