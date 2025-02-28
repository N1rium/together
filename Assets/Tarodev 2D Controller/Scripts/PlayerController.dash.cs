using DG.Tweening;
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
        private Tween _dashTween;
        private Sequence _dashSequence;

        private void CalculateDash()
        {
            if (!Stats.AllowDash || _isSwimming) return;

            if (_dashToConsume && _canDash && !Crouching && _time > _nextDashTime)
            {
                var dir = new Vector2(_frameInput.Move.x, Mathf.Max(_frameInput.Move.y, 0f)).normalized;
                if (dir == Vector2.zero) return;

                _dashVel = dir * Stats.DashVelocity;
                _dashing = true;
                _canDash = false;
                _startedDashing = _time;
                _nextDashTime = _time + Stats.DashCooldown;
                
                SetVelocity(Vector2.zero);
                _dashSequence = DOTween.Sequence();

                _dashSequence.Append(_rb.DOMove(_framePosition, 0.05f));
                
                _dashSequence.Append(_rb.DOMove(_framePosition + dir * 5f, 0.25f).OnUpdate(() =>
                {
                    Debug.Log(_isOnWall);
                    /*if (!_hasWallInFront) return;
                    _dashSequence.Kill();*/
                    /*AddFrameForce(Vector2.up * 20f, true);*/
                }));

                _dashSequence.OnComplete(() =>
                {
                    _dashing = false;
                });
                
                /*_rb.MovePosition(_framePosition + dir * (5f * Time.fixedDeltaTime));*/
                
                DashChanged?.Invoke(true, dir);
            }

            if (!_dashing) return;
            if (!(_time > _startedDashing + Stats.DashDuration)) return;
            
            _dashing = false;
            DashChanged?.Invoke(false, Vector2.zero);

            /*SetVelocity(new Vector2(Velocity.x * Stats.DashEndHorizontalMultiplier, Velocity.y));*/
            if (_grounded) _canDash = true;
        }
    }
}