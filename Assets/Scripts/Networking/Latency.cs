using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    public class Latency : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public float frequency = 0.1f;

        private float _time;

        private void Update()
        {
            _time += Time.deltaTime;
            if (!(_time > frequency)) return;
            UpdateText(GetPing());
            _time = 0f;
        }

        private void UpdateText(float ping)
        {
            if (!_text) return;
            _text.text = $"{ping}ms";
        }

        public float GetPing()
        {
            if (NetworkManager.Singleton == null) return 0f;
            return NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton
                .NetworkConfig.NetworkTransport.ServerClientId);
        }
    }
}
