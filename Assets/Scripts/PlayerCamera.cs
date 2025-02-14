using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private ColliderWrapper _colliderWrapper;
    [SerializeField] private LayerMask _confineLayer;

    private CinemachineCamera _cam;
    private CinemachineConfiner2D _confiner;
    private Collider2D _collider;
    
    void Start()
    {
        _cam = GetComponent<CinemachineCamera>();
        _confiner = _cam.GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        _colliderWrapper.OnTriggerStay += OnPlayerTriggerStay;
        _colliderWrapper.OnTriggerExit += OnPlayerTriggerExit;
    }

    private void OnDestroy()
    {
        _colliderWrapper.OnTriggerStay -= OnPlayerTriggerStay;
        _colliderWrapper.OnTriggerExit -= OnPlayerTriggerExit;
    }

    private void OnPlayerTriggerStay(Collider2D other)
    {
        if (_collider) return;
        if ((_confineLayer.value & (1 << other.gameObject.layer)) == 0) return;
        if (_collider == other) return;
        _confiner.BoundingShape2D = other;
        _collider = other;
    }
    
    private void OnPlayerTriggerExit(Collider2D other)
    {
        if (!_collider) return;
        if (_collider != other) return;
        _confiner.BoundingShape2D = null;
        _collider = null;
    }
}
