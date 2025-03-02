using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        [SerializeField] private ColliderWrapper _deathColliderWrapper;

        private void OnDeathEnter(Collider2D other)
        {
            Debug.Log("DEATH");
        }
    }
}