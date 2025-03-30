using DG.Tweening;
using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private bool _attackToConsume;
        private bool _canAttack = true;
        private float _attackCooldown = 0.5f;

        private bool CanAttack => _attackToConsume && _canAttack && !_dashing;

        private void CalculateAttack()
        {
            if (!CanAttack) return;
            _canAttack = false;
            Attacked?.Invoke(_lastFrameDirection.normalized);
            Delay.For(_attackCooldown).OnComplete(ResetAttack);
        }

        private void ResetAttack()
        {
            _canAttack = true;
        }
    }
}