using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceshipController : MonoBehaviour
{       
    [SerializeField] float rotationSpeed = 3.0f; 
    [SerializeField] float maxRotation = 45.0f;     
        
    
    private float _rotationZ = 0.0f;        

    void Update()
    {        
        float hor = Input.GetAxis("Horizontal");
        
        if (Mathf.Approximately(hor, 0))
        {
            _rotationZ = Mathf.Lerp(_rotationZ, 0, rotationSpeed * Time.deltaTime / 4);
        }
        else
        {
            _rotationZ -= hor * rotationSpeed;
            _rotationZ = Mathf.Clamp(_rotationZ, -maxRotation, maxRotation);
        }        
                        
        transform.localEulerAngles = new Vector3(0, 0, _rotationZ);                                                                                           
    }
}
