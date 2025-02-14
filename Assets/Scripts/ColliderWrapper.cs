using System;
using UnityEngine;

// Used to act as an API for collider enter / exit events
public class ColliderWrapper : MonoBehaviour
{
    private Collider _collider;

    public event Action<Collider2D> OnCollisionEnter;
    public event Action<Collider2D> OnCollisionStay;
    public event Action<Collider2D> OnTriggerEnter;
    public event Action<Collider2D> OnTriggerStay;
    public event Action<Collider2D> OnTriggerExit;
    
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionEnter?.Invoke(other.collider);
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionStay?.Invoke(other.collider);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerStay?.Invoke(other);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExit?.Invoke(other);
    }
}
