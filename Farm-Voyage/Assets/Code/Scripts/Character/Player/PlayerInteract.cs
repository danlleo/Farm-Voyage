using Attributes.WithinParent;
using Common;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerInteract : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField, WithinParent] private Transform _raycastPoint;

        [Header("Settings")] 
        [SerializeField] private float _raycastDistance = 5f;

        private IInteractable _currentInteractable;
        
        public void TryInteract()
        {
            if (Physics.Raycast(_raycastPoint.position, _raycastPoint.forward, out RaycastHit hit, _raycastDistance, _interactableLayerMask))
            {
                if (!hit.collider.TryGetComponent(out IInteractable interactable)) return;
                
                // If we are already interacting with an object, stop interacting with it first
                if (_currentInteractable != null && _currentInteractable != interactable)
                {
                    _currentInteractable.StopInteract();
                }

                // Start interacting with the new object
                _currentInteractable = interactable;
                _currentInteractable.Interact();
            }
            else
            {
                // If the raycast doesn't hit an interactable object and we were interacting with an object, stop interacting
                if (_currentInteractable == null) return;
                
                _currentInteractable.StopInteract();
                _currentInteractable = null;
            }
        }

    }
}