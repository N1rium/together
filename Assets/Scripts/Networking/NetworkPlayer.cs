using System;
using TarodevController;
using TMPro;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    public class NetworkPlayer : NetworkBehaviour, IPlayerActions
    {
        [SerializeField] private TMP_Text nicknameText;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private CinemachineCamera vcam;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerAnimator playerAnimator;
        
        public NetworkVariable<FixedString32Bytes> Nickname = new(writePerm: NetworkVariableWritePermission.Owner);
        public NetworkVariable<Color> Color = new(writePerm: NetworkVariableWritePermission.Owner);

        private MultiplayerManager _multiplayerManager;
        
        public override void OnNetworkSpawn()
        {
            // Subscribe to changes in the Nickname variable
            Nickname.OnValueChanged += OnNicknameChanged;
            Color.OnValueChanged += OnColorChanged;
            
            vcam.enabled = IsOwner;
            _multiplayerManager = GameObject.Find("NetworkManager").GetComponent<MultiplayerManager>();
            playerAnimator.GetComponent<AudioListener>().enabled = IsOwner;
            
            if (IsOwner)
            {
                vcam.transform.SetParent(null);
                DontDestroyOnLoad(vcam.gameObject);

                Nickname.Value = _multiplayerManager.PlayerName;
                Color.Value = _multiplayerManager.Color;
                RegisterPlayerEvents();
                nicknameText.color = new Color(0, 0, 0, 0);
            }
            else
            {
                playerAnimator.SetPlayerActions(this, true);
                playerAnimator.SetNpc(true);
                Destroy(playerInput);
                Destroy(playerController);
                Destroy(GetComponent<ConstantForce2D>());
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<BoxCollider2D>());
                Destroy(GetComponent<CapsuleCollider2D>());
            }
        
            // Ensure nickname is updated initially when the object spawns
            UpdateNicknameText(Nickname.Value.Value);
            UpdatePlayerColor(Color.Value);
        }
    
        private void OnNicknameChanged(FixedString32Bytes prev, FixedString32Bytes curr)
        {
            UpdateNicknameText(curr.Value);
        }
    
        private void OnColorChanged(Color prev, Color curr)
        {
            UpdatePlayerColor(curr);
        }

        private void UpdatePlayerColor(Color c)
        {
            spriteRenderer.color = c;
            playerAnimator.SetPlayerParticleColors(c);
        }
    
        private void UpdateNicknameText(string value)
        {
            nicknameText.text = value;
        }

        private void RegisterPlayerEvents()
        {
            playerController.Jumped += JumpServerRpc;
            playerController.GroundedChanged += GroundedServerRpc;
            playerController.DashChanged += DashServerRpc;
            playerController.WallGrabChanged += WallGrabServerRpc;
            playerController.CrouchingChanged += CrouchServerRpc;
            playerController.SwimmingChanged += SwimmingChanged;
            /*playerController.Repositioned += Repositioned;
            playerController.ToggledPlayer += ToggledPlayer;*/
        }

        private void UnregisterPlayerEvents()
        {
            playerController.Jumped -= JumpServerRpc;
            playerController.GroundedChanged -= GroundedServerRpc;
            playerController.DashChanged -= DashServerRpc;
            playerController.WallGrabChanged -= WallGrabServerRpc;
            playerController.CrouchingChanged -= CrouchServerRpc;
            playerController.SwimmingChanged -= SwimmingChanged;
            /*playerController.Repositioned -= Repositioned;
            playerController.ToggledPlayer -= ToggledPlayer;*/
        }

        #region Jump RPCs

        [Rpc(SendTo.Server)]
        private void JumpServerRpc(JumpType jumpType)
        {
            JumpClientRpc(jumpType);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void JumpClientRpc(JumpType jumpType)
        {
            if (!IsOwner)
            {
                Jumped?.Invoke(jumpType);
            }
        }
        
        #endregion
        
        #region Grounded RPCs
        
        [Rpc(SendTo.Server)]
        private void GroundedServerRpc(bool grounded, float impact)
        {
            GroundedClientRpc(grounded, impact);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void GroundedClientRpc(bool grounded, float impact)
        {
            if (!IsOwner)
            {
                GroundedChanged?.Invoke(grounded, impact);
            }
        }
        
        #endregion
        
        #region Dash RPCs
        
        [Rpc(SendTo.Server)]
        private void DashServerRpc(bool dashing, Vector2 dir)
        {
            DashClientRpc(dashing, dir);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void DashClientRpc(bool dashing, Vector2 dir)
        {
            if (!IsOwner)
            {
                DashChanged?.Invoke(dashing, dir);
            }
        }
        
        #endregion
        
        #region WallGrab RPCs
                
        [Rpc(SendTo.Server)]
        private void WallGrabServerRpc(bool grabbing)
        {
            WallGrabClientRpc(grabbing);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void WallGrabClientRpc(bool grabbing)
        {
            if (!IsOwner)
            {
                WallGrabChanged?.Invoke(grabbing);
            }
        }
        
        #endregion
        
        #region Crouch RPCs
        
        [Rpc(SendTo.Server)]
        private void CrouchServerRpc(bool crouching)
        {
            CrouchClientRpc(crouching);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void CrouchClientRpc(bool crouching)
        {
            if (!IsOwner)
            {
                CrouchingChanged?.Invoke(crouching);
            }
        }
        
        #endregion
        
        #region Swim RPCs
        
        [Rpc(SendTo.Server)]
        private void SwimServerRpc(bool swimming)
        {
            SwimClientRpc(swimming);
        }
        
        [Rpc(SendTo.ClientsAndHost)]
        private void SwimClientRpc(bool swimming)
        {
            if (!IsOwner)
            {
                SwimmingChanged?.Invoke(swimming);
            }
        }
        
        #endregion

        public event Action<JumpType> Jumped;
        public event Action<bool, float> GroundedChanged;
        public event Action<bool, Vector2> DashChanged;
        public event Action<bool> WallGrabChanged;
        public event Action<Vector2> Repositioned;
        public event Action<bool> ToggledPlayer;
        public event Action<bool> SwimmingChanged;
        public event Action<bool> CrouchingChanged;
        public event Action<bool> DeathChanged;

        public event Action Attacked;

        public GeneratedCharacterSize GeneratedCharacterSize()
        {
            return playerController.Stats.CharacterSize.GenerateCharacterSize();
        }
    }
}
