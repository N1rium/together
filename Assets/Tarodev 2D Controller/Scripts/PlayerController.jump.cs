using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private const float JUMP_CLEARANCE_TIME = 0.25f;
        private bool IsWithinJumpClearance => _lastJumpExecutedTime + JUMP_CLEARANCE_TIME > _time;
        private float _lastJumpExecutedTime;
        private bool _bufferedJumpUsable;
        private bool _jumpToConsume;
        private float _timeJumpWasPressed;
        private Vector2 _forceToApplyThisFrame;
        private bool _endedJumpEarly;
        private float _endedJumpForce;
        private int _airJumpsRemaining;
        private bool _wallJumpCoyoteUsable;
        private bool _coyoteUsable;
        private float _timeLeftGrounded;
        private float _returnWallInputLossAfter;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + Stats.BufferedJumpTime && !IsWithinJumpClearance;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _timeLeftGrounded + Stats.CoyoteTime;
        private bool CanAirJump => !_grounded && _airJumpsRemaining > 0;
        private bool CanWallJump => !_grounded && ((_isOnWall || _wallDirThisFrame != 0) || (_wallJumpCoyoteUsable && _time < _timeLeftWall + Stats.WallCoyoteTime));

        private void CalculateJump()
        {
            if (_isSwimming) return;
            if ((_jumpToConsume || HasBufferedJump) && CanStand)
            {
                if (CanWallJump) ExecuteJump(JumpType.WallJump);
                else if (_grounded || ClimbingLadder) ExecuteJump(JumpType.Jump);
                else if (CanUseCoyote) ExecuteJump(JumpType.Coyote);
                else if (CanAirJump) ExecuteJump(JumpType.AirJump);
            }

            if ((!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && Velocity.y > 0) || Velocity.y < 0) _endedJumpEarly = true; // Early end detection


            if (_time > _returnWallInputLossAfter) _wallJumpInputNerfPoint = Mathf.MoveTowards(_wallJumpInputNerfPoint, 1, _delta / Stats.WallJumpInputLossReturnTime);
        }

        private void ExecuteJump(JumpType jumpType)
        {
            SetVelocity(_trimmedFrameVelocity);
            _endedJumpEarly = false;
            _bufferedJumpUsable = false;
            _lastJumpExecutedTime = _time;
            _currentStepDownLength = 0;
            if (ClimbingLadder) ToggleClimbingLadder(false);

            if (jumpType is JumpType.Jump or JumpType.Coyote)
            {
                _coyoteUsable = false;
                AddFrameForce(new Vector2(0, Stats.JumpPower));
            }
            else if (jumpType is JumpType.AirJump)
            {
                _airJumpsRemaining--;
                AddFrameForce(new Vector2(0, Stats.JumpPower));
            }
            else if (jumpType is JumpType.WallJump)
            {
                Debug.Log("WALL JUMP, grounded" + _grounded);
                ToggleOnWall(false);

                _wallJumpCoyoteUsable = false;
                _wallJumpInputNerfPoint = 0;
                _returnWallInputLossAfter = _time + Stats.WallJumpTotalInputLossTime;
                _wallDirectionForJump = _wallDirThisFrame;
                if (_isOnWall || IsPushingAgainstWall)
                {
                    Debug.Log("WallJumpPower");
                    AddFrameForce(new Vector2(-_wallDirThisFrame, 1) * Stats.WallJumpPower);
                }
                else
                {
                    Debug.Log("WallPushPower");
                    AddFrameForce(new Vector2(-_wallDirThisFrame, 1) * Stats.WallPushPower);
                }
            }

            Jumped?.Invoke(jumpType);
        }

        private void ResetAirJumps() => _airJumpsRemaining = Stats.MaxAirJumps;
    }
}