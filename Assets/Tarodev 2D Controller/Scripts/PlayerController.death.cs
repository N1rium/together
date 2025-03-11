using DG.Tweening;
using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        [SerializeField] private ColliderWrapper _deathColliderWrapper;
        private Vector3 _checkpoint;
        private bool CanDie => !_isNoclip;
        
        
        private void Die()
        {
            if (!CanDie) return;
            RepositionImmediately(transform.position, true);
            TogglePlayer(false);
            DeathChanged?.Invoke(true);

            Delay.For(1f).OnComplete(Respawn);
        }

        private void OnDeathEnter(Collider2D other)
        {
            Die();
        }

        private void Respawn()
        {
            RepositionImmediately(_checkpoint, true);
            TogglePlayer(true);
        }

        public void SetCheckpoint(Vector3 pos)
        {
            _checkpoint = pos;
        }
    }
}