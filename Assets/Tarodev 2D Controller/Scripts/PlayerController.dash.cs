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
        private float _nextDashTime;
        private Vector2 _dashDir;

        private Sequence _dashSequence;
        private Collision2D _dashCollision; // Information if we collide during dash

        private void CalculateDash()
        {
            if (!Stats.AllowDash || _isSwimming) return;
            
            if (!_canDash && _time > _nextDashTime && _grounded)
            {
                _canDash = true;
            }

            if (!_dashToConsume || !_canDash || Crouching || !(_time > _nextDashTime)) return;
            
            _dashDir = new Vector2(_frameInput.Move.x, _frameInput.Move.y).normalized;
            if (_dashDir == Vector2.zero) _dashDir = new Vector2(_lastFrameDirection.x, 0f).normalized;

            _dashDir = _dashDir.ToNearestDirection();
                
            _dashing = true;
            _canDash = false;
            _nextDashTime = _time + Stats.DashCooldown;
                
            SetVelocity(Vector2.zero);
            _trimmedFrameVelocity = Vector2.zero;
            _dashSequence = DOTween.Sequence();
                
            _dashSequence.Append(_rb.DOMove(_framePosition + _dashDir * Stats.DashLength, Stats.DashDuration)
                .SetEase(Stats.DashEasingFunction)
                .OnUpdate(() =>
                {
                    if (_dashCollision == null) return;
                        
                    var normal = _dashCollision.contacts[0].normal;
                    var forceDir = (_dashDir * new Vector2(Mathf.Abs(normal.y), Mathf.Abs(normal.x))).normalized;
                        
                    DashComplete(forceDir * Stats.DashCollisionForce);
                }));

            _dashSequence.OnComplete(() =>
            {
                DashComplete(_dashDir * Stats.DashEndPopForce);
            });
                
            DashChanged?.Invoke(true, _dashDir);
        }

        private void DashComplete(Vector2 endForce)
        {
            _dashing = false;
            _dashCollision = null;
            _dashSequence.Kill();
            AddFrameForce(endForce, true);
            DashChanged?.Invoke(false, Vector2.zero);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_dashing) return;
            _dashCollision = other;
        }
    }
}