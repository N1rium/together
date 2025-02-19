using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Networking
{
    public class MultiplayerManager : MonoBehaviour
    {
        [SerializeField] private Image _playerSprite;
        [SerializeField] private TMP_InputField _nameInput;
        [SerializeField] private TMP_Text _joinCodeText;
        
        private NetworkManager _networkManager;

        public string SceneName = "";
        public string PlayerName = "Player";
        public Color[] Colors;
        public int ColorIndex;
        public Color Color;
        public string Code;
    
        void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
        }

        public void SetCode(string code) => Code = code;
        private void OnNameChanged(string nick)
        {
            PlayerName = nick;
        }
    
        private void Start()
        {
            _playerSprite.color = Colors[ColorIndex];
            _nameInput.text = PlayerName;
            _nameInput.onValueChanged.AddListener(OnNameChanged);
            Color = Colors[ColorIndex % Colors.Length];
            
            DontDestroyOnLoad(NetworkManager.Singleton.gameObject);
            _networkManager.OnServerStarted += OnServerStarted;
            
            if (_networkManager.IsHost || _networkManager.IsServer)
            {
                _networkManager.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }
        }

        private void OnDestroy()
        {
            _nameInput.onValueChanged.RemoveAllListeners();
        }

        public void NextColor()
        {
            Color = Colors[++ColorIndex % Colors.Length];
            _playerSprite.color = Color;
            _playerSprite.transform.DOPunchScale(Vector3.one * 0.25f, 0.15f).OnComplete(() =>
            {
                _playerSprite.transform.localScale = Vector3.one;
            });
        }
    
        private void OnServerStarted()
        {
            if (_networkManager.IsServer)
            {
                _networkManager.SceneManager.LoadScene(
                    SceneName, 
                    LoadSceneMode.Single
                );
            }
        }


        private async Task<string> StartHostWithRelay(int maxConnections = 5)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            // Use the first region as an example and create the Relay allocation
            var regions = await RelayService.Instance.ListRegionsAsync();
            var region = regions[0].Id;
            var hostAllocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
            var relayServerData = hostAllocation.ToRelayServerData("dtls");

            _networkManager.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(hostAllocation.AllocationId);
            return _networkManager.StartHost() ? joinCode : null;
        }

        private async Task<bool> StartClientWithRelay(string joinCode)
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var relayServerData = joinAllocation.ToRelayServerData("dtls");
            _networkManager.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            _networkManager.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(PlayerName);
            return !string.IsNullOrEmpty(joinCode) && _networkManager.StartClient();
        }

        public void Matchmake()
        {
            JoinCode();
        }

        public void JoinWithCode()
        {
            JoinCode(Code);
        }

        public void Host()
        {
            Create();
        }

        public async void Create()
        {
            var code = await StartHostWithRelay();
            _joinCodeText.text = code;
            await Api.Post<CodeDto>($"/matchmaking/code/{code}", null);
        }

        public async void JoinCode(string code = "")
        {
            string joinCode = code;
            if (joinCode.Length <= 0)
            {
                var resp = await Api.Get<CodeDto>("/matchmaking/code");
                if (resp.data == null || resp.data.code == null || resp.data.code == "")
                {
                    Debug.LogWarning("No Code Available to join!");
                    return;
                }

                joinCode = resp.data.code;
            }
            
            await StartClientWithRelay(joinCode);
        }
    }

    public sealed class CodeDto
    {
        public string code;
    }
}