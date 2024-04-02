using System;

namespace Character.Player.Events
{
    public class WalkingEvent
    {
        public event EventHandler OnPlayerWalking;

        public void Call(object sender)
        {
            OnPlayerWalking?.Invoke(sender, EventArgs.Empty);
        }
    }
}