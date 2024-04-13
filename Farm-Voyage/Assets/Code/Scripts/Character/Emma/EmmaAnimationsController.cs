using UnityEngine;

namespace Character.Emma
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class EmmaAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Market.Market _market;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _market.ShoppingStateChangedEvent.OnShoppingStateChanged += Market_OnShoppingStateChanged;
        }

        private void OnDisable()
        {
            _market.ShoppingStateChangedEvent.OnShoppingStateChanged -= Market_OnShoppingStateChanged;
        }

        private void Market_OnShoppingStateChanged(bool isShopping)
        {
            if (!isShopping) return;
            
            _animator.SetTrigger(EmmaAnimationsParams.OnGreeting);
        }
    }
}