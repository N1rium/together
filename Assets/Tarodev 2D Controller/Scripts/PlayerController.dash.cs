using DG.Tweening;
using UnityEditor;
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

        private Vector2 _dashDir;
        private float _dashSpeed;

        private void CalculateDash()
        {
            if (!Stats.AllowDash || _isSwimming) return;

            if (_dashToConsume && _canDash && !Crouching && _time > _nextDashTime)
            {
                var dir = new Vector2(_frameInput.Move.x, _frameInput.Move.y).normalized;
                if (dir == Vector2.zero) dir = new Vector2(Mathf.Sign(_lastFrameDirection.x), 0f).normalized;

                _rb.gravityScale = 0f;
                _forceToApplyThisFrame = Vector2.zero;
                _constantForce.force = Vector2.zero;
                _constantForce.enabled = false;
                SetVelocity(Vector2.zero);
                
                _dashVel = dir * Stats.DashVelocity;
                _dashing = true;
                _canDash = false;
                _startedDashing = _time;
                _nextDashTime = _time + Stats.DashCooldown;
                DashChanged?.Invoke(true, dir);

                var pointA = _rb.position;
                var pointB = pointA + _dashVel;
                
                _dashDir = (pointB - pointA).normalized;
                _dashSpeed = Vector3.Distance(pointA, pointB) / Stats.DashDuration;
                
                // Custom method (No down movement while dashing)
                /*DOTween.To(() => 0f, t =>
                    {
                        SetVelocity(_dashDir * (_dashSpeed * t));
                    }, 1f, Stats.DashDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _rb.gravityScale = GRAVITY_SCALE;
                        SetVelocity(Vector2.zero);
                    });*/
                
                // DEEP SEEK METHOD (Going slightly down while dashing)
                SetVelocity(_dashDir * _dashSpeed);
                
                DOVirtual.DelayedCall(Stats.DashDuration, () =>
                {
                    _rb.gravityScale = GRAVITY_SCALE; // Restore gravity
                });
                
            }

            if (!_dashing) return;
            if (!(_time > _startedDashing + Stats.DashDuration)) return;
            
            _dashing = false;
            DashChanged?.Invoke(false, Vector2.zero);
            _constantForce.enabled = true;

            SetVelocity(new Vector2(Velocity.x * Stats.DashEndHorizontalMultiplier, Velocity.y));
            if (_grounded) _canDash = true;
        }
    }
}