using UnityEngine;

namespace Character.Emma
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class Emma : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Market.Market _market;

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
            // TODO: related
        }
    }
}
