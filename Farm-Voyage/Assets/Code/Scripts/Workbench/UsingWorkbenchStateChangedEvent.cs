using System;

namespace Workbench
{
    public class UsingWorkbenchStateChangedEvent
    {
        public event Action<bool> OnUsingWorkbenchStateChanged;

        public void Call(bool isUsingWorkbench)
        {
            OnUsingWorkbenchStateChanged?.Invoke(isUsingWorkbench);
        }
    }
}