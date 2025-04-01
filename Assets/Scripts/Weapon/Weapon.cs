using TarodevController;
using UnityEngine;

namespace Weapon
{
    public class Weapon : MonoBehaviour
    {
        [field: SerializeField] public WeaponStats Stats { get; private set; }
        
        [SerializeField] private GameObject projectile;
        [SerializeField] private AudioSource source;
        
        private IPlayerController _player;

        private readonly Vector3 _rightOffset = new (0.75f,1f,0f);
        private readonly Vector3 _rightRotation = new(0f, 180f, 45f);
        
        private readonly Vector3 _leftOffset = new (-0.75f,1f,0f);
        private readonly Vector3 _leftRotation = new(0f, 0f, 45f);
        
        private ParticleSystem.EmissionModule _emission;

        
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

        private void PlaySound()
        {
            if (Stats == null || source == null) return;
            var sound = Stats.AttackSound.GetSound();
            source.PlayOneShot(sound, 0.125f);
        }

        private void Use(Vector2 dir)
        {
            var go = Instantiate(projectile, transform);
            var t = go.transform;

            PlaySound();
            
            if (dir == Vector2.right)
            {
                t.position = _rightOffset;
                t.rotation = Quaternion.Euler(_rightRotation);
            }
            
            if (dir == Vector2.left)
            {
                t.position = _leftOffset;
                t.rotation = Quaternion.Euler(_leftRotation);
            }

            t.position += transform.position;
        }
    }
}