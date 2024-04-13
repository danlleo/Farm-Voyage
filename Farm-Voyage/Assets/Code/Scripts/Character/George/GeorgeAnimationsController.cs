using UnityEngine;

namespace Character.George
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class GeorgeAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Seller.Seller _seller;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _seller.SellingStateChangedEvent.OnSellingStateChanged += Seller_OnSellingStateChangedStateChanged;
        }

        private void OnDisable()
        {
            _seller.SellingStateChangedEvent.OnSellingStateChanged -= Seller_OnSellingStateChangedStateChanged;
        }

        private void Seller_OnSellingStateChangedStateChanged(bool isSelling)
        {
            if (!isSelling) return;
            
            int salutingAnimationLayer = _animator.GetLayerIndex(GeorgeAnimationLayers.Saluting);
            
            _animator.SetLayerWeight(salutingAnimationLayer, 1f);
            _animator.SetTrigger(GeorgeAnimationParams.OnSalute);
        }
    }
}
