using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{    
    [SerializeField] float horizontalForce = 1.0f;    
    [SerializeField] float verticalSpeed = 1.0f;    
    [SerializeField] float verticalSpeedBoost = 2.0f;    

    [SerializeField] Rigidbody rb;
    [SerializeField] GameManager gameManager;

    private float _speedModifier = 1.0f;

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, 0, verticalSpeed * _speedModifier);
        
        float hor = Input.GetAxis("Horizontal");                     
        Vector3 move = new Vector3(hor * horizontalForce, 0, 0);               
        rb.AddForce(move, ForceMode.Impulse);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _speedModifier = verticalSpeedBoost;
            gameManager.Boost(verticalSpeedBoost);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _speedModifier = 1.0f;
            gameManager.Boost(1.0f);
        }
    }    

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameManager.AddLives(-1);
        }    
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("ScoreLine"))
        {   
            gameManager.AddScore(5);         
            gameManager.AddAsteroid(1);
        }
    }
}
