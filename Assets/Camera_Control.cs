using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Camera_Control : MonoBehaviour
{
    public float lookSpeed;
    public Vector3 newVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h_input = Input.GetAxis("Mouse X") * lookSpeed;
        float v_input = Input.GetAxis("Mouse Y") * lookSpeed;

           newVector = new Vector3 (h_input, v_input, 0);
        
        if (h_input != 0|| v_input != 0)
        {

            transform.Rotate(Vector3.up, h_input, Space.World);     
            transform.Rotate(Vector3.right, -v_input, Space.Self);
        }
        
    }


 
    
}
