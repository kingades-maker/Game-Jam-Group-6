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
      rb.velocity = new Vector3(v_input, rb.velocity.y, h_input);
      
    }
}