using System;
using System.Collections.Generic;
using DG.Tweening;
using Enemy;
using UnityEngine;

namespace Weapon
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float damageTTL = 0.15f;
        [SerializeField] private float TTL = 0.5f;
        [SerializeField] private float DetachTTL = 0.075f;

        [SerializeField] private GameObject hitEffect;
        [SerializeField] private AudioClip hitSoundEffect;

        private Collider2D _collider;
        private AudioSource _audioSource;

        private List<IDamageable> _hits = new();

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
            Delay.For(DetachTTL).OnComplete(() => transform.parent = null);
            Delay.For(damageTTL).OnComplete(Deactivate);
            Delay.For(TTL).OnComplete(Kill);
        }

        private void Deactivate()
        {
            _collider.enabled = false;
        }

        private void Kill()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<IDamageable>(out var damageable)) return;
            if (_hits.Contains(damageable)) return;
            
            _hits.Add(damageable);
            
            var go = Instantiate(hitEffect);
            _audioSource.PlayOneShot(hitSoundEffect, 0.125f);
            var collisionPos = other.ClosestPoint(transform.position);
            var dir = (collisionPos - (Vector2)other.transform.position).normalized;
            go.transform.position = collisionPos;
            /*go.transform.rotation = Quaternion.LookRotation(dir, Vector3.right);*/
            damageable.TakeDamage(new()
            {
                Damage = 1f,
                Position = collisionPos
            });
        }
    }
}