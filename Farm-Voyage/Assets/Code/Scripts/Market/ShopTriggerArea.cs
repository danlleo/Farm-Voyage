using Character.Player;
using UnityEngine;

namespace Market
{
    [DisallowMultipleComponent]
    public class ShopTriggerArea : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private Market _market;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player _)) return;
            _market.ShoppingStateChangedEvent.Call(true);
        }
    }
}
