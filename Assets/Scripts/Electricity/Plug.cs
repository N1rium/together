using System;
using DG.Tweening;
using UnityEngine;

public class Plug : MonoBehaviour, IInteractable
{
    private Rigidbody2D _rb;
    private Transform _parent;

    private bool _isInteracting;

    public event Action OnDone;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _parent = transform.parent;
    }
    
    public void OnInteract(Transform other)
    {
        if (_isInteracting)
        {
            CancelInteraction();
            return;
        }
        
        _rb.bodyType = RigidbodyType2D.Kinematic;
        transform.SetParent(other.transform);
        _isInteracting = true;
    }

    public bool GetIsBusy()
    {
        return _isInteracting;
    }

    private void CancelInteraction()
    {
        transform.SetParent(_parent);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        OnDone?.Invoke();
        _isInteracting = false;
    }
}
