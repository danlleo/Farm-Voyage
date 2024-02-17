using Character.Player;
using UnityEngine;

namespace Market
{
    [DisallowMultipleComponent]
    public class ShopTriggerArea : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Player player)) return;
        }
    }
}
