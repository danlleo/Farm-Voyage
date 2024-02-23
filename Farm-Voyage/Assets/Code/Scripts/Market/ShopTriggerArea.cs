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
            if (!other.TryGetComponent(out Player player)) return;
            _market.StartedShoppingEvent.Call(this);
        }
    }
}
