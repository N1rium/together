using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace TarodevController
{
    public class PlayerInput : MonoBehaviour
    {
#if ENABLE_INPUT_SYSTEM
        private PlayerInputActions _actions;
        private InputAction _move, _jump, _dash, _horizontal, _vertical, _grab, _interact, _noClip, _attack;

        private void Awake()
        {
            _actions = new PlayerInputActions();
            var player = _actions.Player;
            _move = player.Move;
            _jump = player.Jump;
            _dash = player.Dash;
            _vertical = player.Vertical;
            _horizontal = player.Horizontal;
            _grab = player.Grab;
            _interact = player.Interact;
            _noClip = player.Noclip;
            _attack = player.Attack;
        }

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();

        public FrameInput Gather()
        {
            // TODO - Fix so A and D are Horizontal and W and S are Vertical
            var move = new Vector2(_horizontal.ReadValue<float>(), _vertical.ReadValue<float>());
            if (move == Vector2.zero)
            {
                move = _move.ReadValue<Vector2>();
            }
            
            return new FrameInput
            {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                DashDown = _dash.WasPressedThisFrame(),
                GrabHeld = _grab.IsPressed(),
                Interact = _interact.WasPressedThisFrame(),
                Move = move,
                NoClipDown = _noClip.WasPressedThisFrame(),
                Attack = _attack.WasPressedThisFrame(),
            };
        }
        
        public PlayerInputActions GetActions() => _actions;
#else
    public FrameInput Gather()
        {
            return new FrameInput
            {
                JumpDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.C),
                DashDown = Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };
        }
#endif
    }

    public struct FrameInput
    {
        public Vector2 Move;
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool GrabHeld;
        public bool Interact;
        public bool NoClipDown;
        public bool Attack;
    }
}