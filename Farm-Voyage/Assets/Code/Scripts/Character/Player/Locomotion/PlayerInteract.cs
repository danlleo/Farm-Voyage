using Attributes.WithinParent;
using Common;
using UnityEngine;

namespace Character.Player.Locomotion
{
    [DisallowMultipleComponent]
    public class PlayerInteract : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField, WithinParent] private Transform _raycastPoint;

        [Header("Settings")] 
        [SerializeField, Min(0.1f)] private float _castDistance = 5f;
        [SerializeField, Min(0.1f)] private float _sphereRadius = 1f;
        
        private IInteractable _currentInteractable;
        
        public void TryInteract()
        {
            if (Physics.SphereCast(_raycastPoint.position, _sphereRadius, _raycastPoint.forward, out RaycastHit hit,
                    _castDistance, _interactableLayerMask))
            {
                if (!hit.collider.TryGetComponent(out IInteractable interactable)) return;
                
                IStopInteractable stopInteractable = _currentInteractable as IStopInteractable;
                
                // If we are already interacting with an object, stop interacting with it first
                if (_currentInteractable != null && _currentInteractable != interactable && stopInteractable != null)
                {
                    stopInteractable.StopInteract();
                }
            
                // Start interacting with the new object
                _currentInteractable = interactable;
                _currentInteractable.Interact();
            }
            else
            {
                // If the raycast doesn't hit an interactable object and we were interacting with an object, stop interacting
                if (_currentInteractable == null) return;
                
                if (_currentInteractable is IStopInteractable stopInteractable)
                {
                    stopInteractable.StopInteract();
                }
                
                _currentInteractable = null;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 spherePosition = _raycastPoint.position + _raycastPoint.forward * _castDistance;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_raycastPoint.position, spherePosition);
            Gizmos.DrawWireSphere(spherePosition, _sphereRadius);
        }
#endif
    }
}