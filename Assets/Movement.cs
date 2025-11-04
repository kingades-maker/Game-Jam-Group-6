using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    void Start()
    {;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
      float h_input = Input.GetAxis("Horizontal") * speed;
        float v_input = Input.GetAxis("Vertical") * speed;
      rb.linearVelocity = new Vector3(v_input, rb.linearVelocity.y, h_input);
      
    }
}