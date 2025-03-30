using DG.Tweening;
using Rooms;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IRoomObject
    {
        private Vector3 _startPos;
        private SpriteRenderer[] _renderers;

        private Vector3 _startScale;
        
        private void Awake()
        {
            _startScale = transform.localScale;
            _startPos = transform.position;
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }
        
        public void OnRoomEnter(RoomData data)
        {
            gameObject.SetActive(true);
        }

        public void OnRoomExit(RoomData data)
        {
            gameObject.SetActive(false);
            ResetTransform();
        }
        
        private void ResetTransform()
        {
            var t = transform;
            t.position = _startPos;
        }

        protected void OnHit()
        {
            transform.DOShakeScale(0.15f, Vector3.one * 0.25f)
                .OnComplete(() => transform.localScale = _startScale);
            
            foreach (var r in _renderers)
            {
                r.material.DOFloat(0f, "_Flash", 0.3f).From(1f);
            }
        }
    }
}