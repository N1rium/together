using DG.Tweening;
using Extension;
using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private bool _dashToConsume;
        private bool _canDash;
        private bool _dashing;
        private float _startedDashing;
        private float _nextDashTime;
        private Vector2 _dashDir;

        private Sequence _dashSequence;
        private Collision2D _dashCollision; // Information if we collide during dash

        private void CalculateDash()
        {
            if (!Stats.AllowDash || _isSwimming) return;

            if (_dashToConsume && _canDash && !Crouching && _time > _nextDashTime)
            {
                _dashDir = new Vector2(_frameInput.Move.x, _frameInput.Move.y).normalized;
                if (_dashDir == Vector2.zero) _dashDir = new Vector2(_lastFrameDirection.x, 0f).normalized;

                _dashDir = _dashDir.ToNearestDirection();
                
                _dashing = true;
                _canDash = false;
                _startedDashing = _time;
                _nextDashTime = _time + Stats.DashCooldown;
                
                SetVelocity(Vector2.zero);
                _trimmedFrameVelocity = Vector2.zero;
                _dashSequence = DOTween.Sequence();
                
                _dashSequence.Append(_rb.DOMove(_framePosition + _dashDir * Stats.DashLength, Stats.DashDuration)
                    .SetEase(Ease.InQuad)
                    .OnUpdate(() =>
                    {
                        if (_dashCollision == null) return;
                        _dashing = false;
                        _dashSequence.Kill();
                        DashChanged?.Invoke(false, Vector2.zero);
                        
                        var normal = _dashCollision.contacts[0].normal;
                        var forceDir = (_dashDir * new Vector2(Mathf.Abs(normal.y), Mathf.Abs(normal.x))).normalized;
                        
                        AddFrameForce(forceDir * Stats.DashCollisionForce, true);
                        _dashCollision = null;
                    }));

                _dashSequence.OnComplete(() =>
                {
                    _dashing = false;
                    AddFrameForce(_dashDir * Stats.DashEndPopForce, true);
                    _dashCollision = null;
                    DashChanged?.Invoke(false, Vector2.zero);
                });
                
                DashChanged?.Invoke(true, _dashDir);
            }

            if (!_dashing) return;
            if (!(_time > _startedDashing + Stats.DashDuration)) return;
            
            _dashing = false;
            DashChanged?.Invoke(false, Vector2.zero);

            SetVelocity(new Vector2(Velocity.x * Stats.DashEndHorizontalMultiplier, Velocity.y));
            if (_grounded) _canDash = true;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_dashing) return;
            _dashCollision = other;
        }
    }
}