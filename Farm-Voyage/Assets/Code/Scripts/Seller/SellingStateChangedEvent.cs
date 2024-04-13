using System;

namespace Seller
{
    public class SellingStateChangedEvent
    {
        public event Action<bool> OnSellingStateChanged;

        public void Call(bool isSelling)
        {
            OnSellingStateChanged?.Invoke(isSelling);
        }
    }
}