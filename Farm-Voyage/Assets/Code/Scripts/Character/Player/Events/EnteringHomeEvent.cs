using System;

namespace Character.Player.Events
{
    public class EnteringHomeEvent
    {
        public event EventHandler OnPlayerEnteringHome;

        public void Call(object sender)
        {
            OnPlayerEnteringHome?.Invoke(sender, EventArgs.Empty);
        }
    }
}