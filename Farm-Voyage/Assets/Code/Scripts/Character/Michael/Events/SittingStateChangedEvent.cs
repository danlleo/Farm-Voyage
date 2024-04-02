using System;

namespace Character.Michael.Events
{
    public class SittingStateChangedEvent
    {
        public event Action<bool> OnMichaelSittingStateChanged;

        public void Call(bool isSitting)
        {
	        OnMichaelSittingStateChanged?.Invoke(isSitting);
        }
    }
}