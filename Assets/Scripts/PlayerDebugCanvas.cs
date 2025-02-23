using System;
using TarodevController;
using TMPro;
using UnityEngine;

public class PlayerDebugCanvas : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private TMP_Text _groundedText;
    [SerializeField] private TMP_Text _wallGrabText;
    [SerializeField] private TMP_Text _rbVelocityText;
    void Start()
    {
        _playerController.GroundedChanged += OnGroundedChanged;
    }

    private void Update()
    {
        _wallGrabText.text = $"Wall grab: {_playerController.IsGrabbingWall}";
        _rbVelocityText.text = $"Velocity: ({_playerController.Velocity.x}, {_playerController.Velocity.y})";
    }

    private void OnDisable()
    {
        _playerController.GroundedChanged -= OnGroundedChanged;
    }

    private void OnGroundedChanged(bool grounded, float impact)
    {
        _groundedText.text = $"Grounded: {grounded}";
    }
}
