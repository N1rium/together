using UnityEngine;

namespace Enemy
{
    public class Walker : EnemyBase, IDamageable
    {
        private Rigidbody2D _rb;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            /*_rb = GetComponent<Rigidbody2D>();*/
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void TakeDamage(DamageData data)
        {
            OnHit();
            /*var dir = (transform.position - data.Position).normalized;
            _rb.AddForce(dir * 2f, ForceMode2D.Impulse);*/
        }
    }
}
