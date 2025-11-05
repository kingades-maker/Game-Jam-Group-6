using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    void Start()
    {
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
      
    }
}