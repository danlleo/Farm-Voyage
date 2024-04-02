using System;

namespace Character.Player.Events
{
    public class LeftHomeEvent 
    {
        public event EventHandler OnPlayerLeftHome;

        public void Call(object sender)
        {
            OnPlayerLeftHome?.Invoke(sender, EventArgs.Empty);
        }
    }
}