using System;
using TarodevController;
using UnityEngine;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        [field: SerializeField] public WeaponStats Stats { get; private set; }
        private IPlayerController _player;
        
        private void Awake()
        {
            _player = GetComponentInParent<IPlayerController>();
        }

        private void OnEnable()
        {
            _player.Attacked += Use;
        }

        private void OnDisable()
        {
            _player.Attacked -= Use;
        }

        public void Use()
        {
            
        }
    }
}