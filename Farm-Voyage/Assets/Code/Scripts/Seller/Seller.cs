using UnityEngine;

namespace Seller
{
    [RequireComponent(typeof(StartedSellingEvent))]
    [DisallowMultipleComponent]
    public class Seller : MonoBehaviour
    {
        public StartedSellingEvent StartedSellingEvent { get; private set; }

        private void Awake()
        {
            StartedSellingEvent = GetComponent<StartedSellingEvent>();
        }
    }
}