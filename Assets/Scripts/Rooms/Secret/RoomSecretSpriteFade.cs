using DG.Tweening;
using UnityEngine;

namespace Rooms.Secret
{
    public class RoomSecretSpriteFade : MonoBehaviour, IRoomSecretObject
    {
        private SpriteRenderer _sr;
        private bool _triggered;

        private void Start()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void OnRevealed(float suspenseTime)
        {
            if (_triggered) return;
            _triggered = true;
            _sr.DOFade(0f, suspenseTime).From(1f).SetEase(Ease.Linear);
        }
    }
}
