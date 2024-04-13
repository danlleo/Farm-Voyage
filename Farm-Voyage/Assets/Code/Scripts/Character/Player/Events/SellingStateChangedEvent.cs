using System;

namespace Character.Player.Events
{
    public class SellingStateChangedEvent
    {
        public event Action<bool> OnStartedSellingStateChanged;

        public void Call(bool isSelling)
        {
            OnStartedSellingStateChanged?.Invoke(isSelling);
        }
    }
}