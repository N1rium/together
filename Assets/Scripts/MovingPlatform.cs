using System;
using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 _lastPosition;
    private Vector3 _currentVelocity;

    void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        transform.position += Vector3.right * (1f * Time.deltaTime);
        /*_currentVelocity = (transform.position - _lastPosition);
        _lastPosition = transform.position;*/
    }

    void FixedUpdate()
    {
        // Calculate how much the platform moved this frame
        _currentVelocity = (transform.position - _lastPosition);
        _lastPosition = transform.position;
    }

    // Called when another object stays on the platform
    /*void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Move the player with the platform's velocity
                playerRb.velocity += _currentVelocity;
            }
        }
    }*/

    public Vector3 GetVelocity() => _currentVelocity;
}
