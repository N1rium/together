using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        [SerializeField] private ColliderWrapper _swimColliderWrapper;
        private bool _isSwimming;
        private Collider2D _waterHit;
        private void CalculateSwim()
        {
            if (!Stats.AllowSwimming || !_isSwimming) return;
            
            // TODO - Custom management for Swim dashing
            if (_jumpToConsume)
            {
                AddFrameForce(_frameInput.Move * Stats.SwimDashPower);
            }
        }
        
        private void OnWaterEnter(Collider2D other)
        {
            _isSwimming = true;
            SwimmingChanged?.Invoke(true);
        }
        
        private void OnWaterLeave(Collider2D other)
        {
            _isSwimming = false;
            AddFrameForce(_frameInput.Move * 10f);
            SwimmingChanged?.Invoke(false);
        }

        // Might need to be moved to main class file
        private void OnEnable()
        {
            _swimColliderWrapper.OnTriggerEnter += OnWaterEnter;
            _swimColliderWrapper.OnTriggerExit += OnWaterLeave;
        }

        private void OnDisable()
        {
            _swimColliderWrapper.OnTriggerEnter -= OnWaterEnter;
            _swimColliderWrapper.OnTriggerExit -= OnWaterLeave;
        }
    }
}