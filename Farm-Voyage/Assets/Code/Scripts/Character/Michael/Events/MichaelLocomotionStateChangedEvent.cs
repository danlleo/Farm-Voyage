using System;

namespace Character.Michael.Events
{
    public class MichaelLocomotionStateChangedEvent 
    {
        public event Action<bool> OnMichaelWalking;

        public void Call(bool isWalking)
        {
            OnMichaelWalking?.Invoke(isWalking);
        }
    }
}