using System;

namespace Character.Player.Events
{
    public class UsingWorkbenchStateChangedEvent
    {
        public event Action<bool> OnPlayerUsingWorkbenchStateChanged;

        public void Call(bool isUsingWorkbench)
        {
            OnPlayerUsingWorkbenchStateChanged?.Invoke(isUsingWorkbench);
        }
    }
}