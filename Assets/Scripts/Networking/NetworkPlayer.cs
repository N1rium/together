using Networking;
using TarodevController;
using TMPro;
using Unity.Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CinemachineCamera vcam;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerController playerController;
    
    public NetworkVariable<FixedString32Bytes> Nickname = new(writePerm: NetworkVariableWritePermission.Owner);
    public NetworkVariable<Color> Color = new(writePerm: NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        // Subscribe to changes in the Nickname variable
        Nickname.OnValueChanged += OnNicknameChanged;
        Color.OnValueChanged += OnColorChanged;
        
        playerInput.enabled = IsOwner;
        playerController.enabled = IsOwner;
        vcam.enabled = IsOwner;
            
        if (IsOwner)
        {
            vcam.transform.SetParent(null);
            DontDestroyOnLoad(vcam.gameObject);

            var manager = GameObject.Find("NetworkManager").GetComponent<MultiplayerManager>();
            Nickname.Value = manager.PlayerName;
            Color.Value = manager.Color;
        }
        
        // Ensure nickname is updated initially when the object spawns
        UpdateNicknameText(Nickname.Value.Value);
    }
    
    private void OnNicknameChanged(FixedString32Bytes prev, FixedString32Bytes curr)
    {
        UpdateNicknameText(curr.Value);
    }
    
    private void OnColorChanged(Color prev, Color curr)
    {
        spriteRenderer.color = curr;
    }
    
    private void UpdateNicknameText(string value)
    {
        nicknameText.text = value;
    }
}
