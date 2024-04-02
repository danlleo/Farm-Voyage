using System;

namespace Character.Michael.Events
{
    public class MichaelSittingStateChangedEvent
    {
        public event Action<bool> OnMichaelSittingStateChanged;

        public void Call(bool isSitting)
        {
	        OnMichaelSittingStateChanged?.Invoke(isSitting);
        }
    }
}