using System;
using Attributes.WithinParent;
using Common;
using UnityEngine;

namespace Character.Player.Locomotion
{
    [DisallowMultipleComponent]
    public class PlayerInteract : MonoBehaviour
    {
        public static event Action<float, float> OnAnyInteractDisplayProgressSpotted;
        public static event Action OnAnyInteractDisplayProgressLost; 
        
        [Header("External references")] 
        [SerializeField] private Player _player;
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField, WithinParent] private Transform _raycastPoint;

        [Header("Settings")] 
        [SerializeField, Min(0.1f)] private float _castDistance = 5f;
        [SerializeField, Min(0.1f)] private float _sphereRadius = 1f;
        
        private IInteractable _currentInteractable;
        
        public void TryInteract()
        {
            if (Physics.SphereCast(_raycastPoint.position, _sphereRadius, _raycastPoint.forward, 
                    out RaycastHit hit,
                    _castDistance, _interactableLayerMask))
            {
                if (!hit.collider.TryGetComponent(out IInteractable interactable)) return;
                
                IPlayerStopInteractable playerStopInteractable = _currentInteractable as IPlayerStopInteractable;
                IStopInteractable stopInteractable = _currentInteractable as IStopInteractable;
                IInteractDisplayProgress interactDisplayProgress = _currentInteractable as IInteractDisplayProgress;
                
                StopPlayerInteractable(interactable, playerStopInteractable);
                StopInteractable(interactable, stopInteractable);
                SetNewAndInteract(interactable);
                TrySpotInteractDisplayProgress(interactDisplayProgress);
            }
            else
            {
                // If the raycast doesn't hit an interactable object and we were interacting with an object, stop interacting
                if (_currentInteractable == null) return;
                
                IPlayerStopInteractable playerStopInteractable = _currentInteractable as IPlayerStopInteractable;
                playerStopInteractable?.PlayerStopInteract(_player);

                IStopInteractable stopInteractable = _currentInteractable as IStopInteractable;
                stopInteractable?.StopInteract();
                
                IInteractDisplayProgress interactDisplayProgress = _currentInteractable as IInteractDisplayProgress;

                if (interactDisplayProgress != null)
                {
                    OnAnyInteractDisplayProgressLost?.Invoke();
                }
                
                _currentInteractable = null;
            }
        }

        private static void TrySpotInteractDisplayProgress(IInteractDisplayProgress interactDisplayProgress)
        {
            if (interactDisplayProgress == null) return;
            float currentClampedProgress = Mathf.Clamp(interactDisplayProgress.CurrentClampedProgress, 0f, 1f);
            float maxClampedProgress = Mathf.Clamp(interactDisplayProgress.MaxClampedProgress, 0f, 1f);

            OnAnyInteractDisplayProgressSpotted?.Invoke(currentClampedProgress, maxClampedProgress);
        }

        private void SetNewAndInteract(IInteractable interactable)
        {
            _currentInteractable = interactable;
            _currentInteractable.Interact(_player);
        }

        private void StopInteractable(IInteractable interactable, IStopInteractable stopInteractable)
        {
            if (_currentInteractable != null && _currentInteractable != interactable && stopInteractable != null)
            {
                stopInteractable.StopInteract();
            }
        }

        private void StopPlayerInteractable(IInteractable interactable, IPlayerStopInteractable playerStopInteractable)
        {
            if (_currentInteractable != null && _currentInteractable != interactable && playerStopInteractable != null)
            {
                playerStopInteractable.PlayerStopInteract(_player);
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