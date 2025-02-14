using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private bool _dashToConsume;
        private bool _canDash;
        private Vector2 _dashVel;
        private bool _dashing;
        private float _startedDashing;
        private float _nextDashTime;

        private void CalculateDash()
        {
            if (!Stats.AllowDash) return;

            if (_dashToConsume && _canDash && !Crouching && _time > _nextDashTime)
            {
                var dir = new Vector2(_frameInput.Move.x, Mathf.Max(_frameInput.Move.y, 0f)).normalized;
                if (dir == Vector2.zero) return;

                _dashVel = dir * Stats.DashVelocity;
                _dashing = true;
                _canDash = false;
                _startedDashing = _time;
                _nextDashTime = _time + Stats.DashCooldown;
                DashChanged?.Invoke(true, dir);
            }

            if (_dashing)
            {
                if (_time > _startedDashing + Stats.DashDuration)
                {
                    _dashing = false;
                    DashChanged?.Invoke(false, Vector2.zero);

                    SetVelocity(new Vector2(Velocity.x * Stats.DashEndHorizontalMultiplier, Velocity.y));
                    if (_grounded) _canDash = true;
                }
            }
        }
    }
}