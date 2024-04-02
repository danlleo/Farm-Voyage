using System;

namespace Character.Player.Events
{
    public class IdleEvent
    {
        public event EventHandler OnPlayerIdle;

        public void Call(object sender)
        {
            OnPlayerIdle?.Invoke(sender, EventArgs.Empty);
        }
    }
}