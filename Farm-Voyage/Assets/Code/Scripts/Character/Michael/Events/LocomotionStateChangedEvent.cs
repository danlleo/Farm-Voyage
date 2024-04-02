using System;

namespace Character.Michael.Events
{
    public class LocomotionStateChangedEvent 
    {
        public event Action<bool> OnMichaelWalking;

        public void Call(bool isWalking)
        {
            OnMichaelWalking?.Invoke(isWalking);
        }
    }
}