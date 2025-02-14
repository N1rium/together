using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private float _timeStartedCrouching;
        private bool CrouchPressed => _frameInput.Move.y < -Stats.VerticalDeadZoneThreshold;

        private bool CanStand => IsStandingPosClear(_rb.position + _character.StandingColliderCenter);
        private bool IsStandingPosClear(Vector2 pos) => CheckPos(pos, _character.StandingColliderSize - SKIN_WIDTH * Vector2.one);

        // We handle crouch AFTER frame movements are done to avoid transient velocity issues
        private void CalculateCrouch()
        {
            if (!Stats.AllowCrouching) return;

            if (!Crouching && CrouchPressed && _grounded) ToggleCrouching(true);
            else if (Crouching && (!CrouchPressed || !_grounded)) ToggleCrouching(false);
        }

        private void ToggleCrouching(bool shouldCrouch)
        {
            if (shouldCrouch)
            {
                _timeStartedCrouching = _time;
                Crouching = true;
            }
            else
            {
                if (!CanStand) return;
                Crouching = false;
            }

            SetColliderMode(Crouching ? ColliderMode.Crouching : ColliderMode.Standard);
        }

        private bool CheckPos(Vector2 pos, Vector2 size)
        {
            Physics2D.queriesHitTriggers = false;
            var hit = Physics2D.OverlapBox(pos, size, 0, Stats.CollisionLayers);
            //var hit = Physics2D.OverlapCapsule(pos, size - new Vector2(SKIN_WIDTH, 0), _collider.direction, 0, ~Stats.PlayerLayer);
            Physics2D.queriesHitTriggers = _cachedQueryMode;
            return !hit;
        }
    }
}