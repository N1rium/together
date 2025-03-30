using UnityEngine;

namespace Enemy
{
    public interface IDamageable
    {
        public void TakeDamage(DamageData data);
    }

    public class DamageData
    {
        public float Damage;
        public Vector3 Position;
    }
}