using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private bool CanEnterLadder => _ladderHit && _time > _timeLeftLadder + Stats.LadderCooldownTime;
        private bool ShouldMountLadder => Stats.AutoAttachToLadders || _frameInput.Move.y > Stats.VerticalDeadZoneThreshold || (!_grounded && _frameInput.Move.y < -Stats.VerticalDeadZoneThreshold);
        private bool ShouldDismountLadder => !Stats.AutoAttachToLadders && _grounded && _frameInput.Move.y < -Stats.VerticalDeadZoneThreshold;

        private float _timeLeftLadder;
        private Collider2D _ladderHit;
        private float _ladderSnapVel;

        private void CalculateLadders()
        {
            if (!Stats.AllowLadders) return;

            Physics2D.queriesHitTriggers = true; // Ladders are set to Trigger
            _ladderHit = Physics2D.OverlapBox(_framePosition + (Vector2)_wallDetectionBounds.center, _wallDetectionBounds.size, 0, Stats.LadderLayer);

            Physics2D.queriesHitTriggers = _cachedQueryTriggers;

            if (!ClimbingLadder && CanEnterLadder && ShouldMountLadder) ToggleClimbingLadder(true);
            else if (ClimbingLadder && (!_ladderHit || ShouldDismountLadder)) ToggleClimbingLadder(false);
        }

        private void ToggleClimbingLadder(bool on)
        {
            if (ClimbingLadder == on) return;
            if (on)
            {
                SetVelocity(Vector2.zero);
                _rb.gravityScale = 0;
                _ladderSnapVel = 0; // reset damping velocity for consistency
            }
            else
            {
                if (_ladderHit) _timeLeftLadder = _time; // to prevent immediately re-mounting ladder
                if (_frameInput.Move.y > 0)
                {
                    AddFrameForce(new Vector2(0, Stats.LadderPopForce));
                }

                _rb.gravityScale = GRAVITY_SCALE;
            }

            ClimbingLadder = on;
            ResetAirJumps();
        }  
    }
}