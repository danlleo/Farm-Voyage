using System;

namespace Market
{
    public class ShoppingStateChangedEvent
    {
        public event Action<bool> OnShoppingStateChanged;

        public void Call(bool isShopping)
        {
            OnShoppingStateChanged?.Invoke(isShopping);
        }
    }
}