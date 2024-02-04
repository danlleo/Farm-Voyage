using Attributes.ChildrenOnly;
using Common;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerInteract : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private LayerMask _interactableLayerMask;
        [SerializeField, ChildrenOnly] private Transform _raycastPoint;

        [Header("Settings")] 
        [SerializeField] private float _raycastDistance = 5f;
        
        public void TryInteract()
        {
            RaycastHit hit;

            if (!Physics.Raycast(_raycastPoint.position, _raycastPoint.forward, out hit, _raycastDistance))
                return;
            
            var interactable = hit.collider.GetComponent<IInteractable>();

            interactable?.Interact();
        }
    }
}