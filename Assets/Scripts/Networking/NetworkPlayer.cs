using System;
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
    public NetworkVariable<FixedString32Bytes> ColorString = new(writePerm: NetworkVariableWritePermission.Owner);

    /*private void Awake()
    {
        playerInput.enabled = false;
        playerController.enabled = false;
        vcam.enabled = false;
    }*/

    public override void OnNetworkSpawn()
    {
        // Subscribe to changes in the Nickname variable
        Nickname.OnValueChanged += OnNicknameChanged;
        ColorString.OnValueChanged += OnColorChanged;
        
        playerInput.enabled = IsOwner;
        playerController.enabled = IsOwner;
        vcam.enabled = IsOwner;
            
        if (IsOwner)
        {
            vcam.transform.SetParent(null);
            DontDestroyOnLoad(vcam.gameObject);
                
            var data = System.Text.Encoding.Default.GetString(NetworkManager.NetworkConfig.ConnectionData);
            var split = data.Split(":");
            Nickname.Value = split[0];

            // R:G:B
            ColorString.Value = $"{split[1]}:{split[2]}:{split[3]}";
        }
        
        // Ensure nickname is updated initially when the object spawns
        UpdateNicknameText(Nickname.Value.Value);
    }
    
    private void OnNicknameChanged(FixedString32Bytes prev, FixedString32Bytes curr)
    {
        UpdateNicknameText(curr.Value);
    }
    
    private void OnColorChanged(FixedString32Bytes prev, FixedString32Bytes curr)
    {
        var rgba = curr.Value.Split(":");
        var color = new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), 1f);
        spriteRenderer.color = color;
    }
    
    private void UpdateNicknameText(string value)
    {
        nicknameText.text = value;
    }
}
