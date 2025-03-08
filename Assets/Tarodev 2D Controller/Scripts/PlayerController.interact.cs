using System;
using System.Collections.Generic;
using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        [SerializeField] private ColliderWrapper _interactColliderWrapper;
        private List<IInteractable> _interactions = new();
        private IInteractable _interaction;
        
        private bool CanInteract => _frameInput.Interact && !_dashing && _interactions.Count > 0;
        
        private void CalculateInteraction()
        {
            if (!CanInteract) return;
            var interaction = _interaction ?? _interactions[0];
            interaction.OnInteract(transform);

            if (_interaction != null) return;
            SetInteraction(interaction);
        }

        private void SetInteraction(IInteractable interaction)
        {
            if (_interaction != null)
            {
                _interaction.OnDone -= OnInteractionDone;
            }
            
            _interaction = interaction;
            _interaction.OnDone += OnInteractionDone;
        }

        private void OnInteractionDone()
        {
            
        }

        private void OnInteractableEnter(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable item)) _interactions.Add(item);
            Debug.Log(_interactions.Count);
        }
        
        private void OnInteractableLeave(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable item))
            {
                if (_interaction == item) _interaction = null;
                _interactions.Remove(item);
            }
            Debug.Log(_interactions.Count);
        }
    }
}