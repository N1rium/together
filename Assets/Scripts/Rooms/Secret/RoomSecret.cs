using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms.Secret
{
    public class RoomSecret : MonoBehaviour
    {
        [SerializeField] private List<IRoomSecretObject> objects;

        private Collider2D _collider;
    
        void Start()
        {
            _collider = GetComponent<Collider2D>();
            objects = GetComponentsInChildren<IRoomSecretObject>().ToList();
        }

        public void Reveal()
        {
            foreach (var obj in objects)
            {
                obj.OnRevealed(0.5f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Reveal();
        }
    }
}
