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
            if ((Stats.WaterLayer.value & (1 << other.gameObject.layer)) == 0) return;
            _isSwimming = true;
            SwimmingChanged?.Invoke(true);
        }
        
        private void OnWaterLeave(Collider2D other)
        {
            if ((Stats.WaterLayer.value & (1 << other.gameObject.layer)) == 0) return;
            _isSwimming = false;
            AddFrameForce(_frameInput.Move * 10f);
            SwimmingChanged?.Invoke(false);
        }
    }
}