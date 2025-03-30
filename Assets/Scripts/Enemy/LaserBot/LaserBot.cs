using DG.Tweening;
using UnityEngine;

namespace Enemy
{
    public class LaserBot : EnemyBase, IDamageable
    {
        [SerializeField] private Transform bodyTransform;
        [SerializeField] private Transform target;

        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxSpeed = 20f;

        private Vector3 _vel;
        private Rigidbody2D _rb;
    
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            bodyTransform.DOLocalRotate(Vector3.forward * 360f, 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        void Update()
        {
            /*transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _vel, speed * Time.deltaTime);*/
        }

        private void FixedUpdate()
        {
            var dir = (target.position - transform.position).normalized;
            _rb.AddForce(dir * speed);
        
            if (_rb.linearVelocity.magnitude > maxSpeed)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
            }
        }

        public void TakeDamage(DamageData data)
        {
            OnHit();
            var dir = (transform.position - data.Position).normalized;
            _rb.AddForce(dir * 10f, ForceMode2D.Impulse);
        }
    }
}
