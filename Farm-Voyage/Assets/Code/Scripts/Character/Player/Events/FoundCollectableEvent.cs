using System;

namespace Character.Player.Events
{
    public class FoundCollectableEvent
    {
        public event EventHandler OnPlayerFoundCollectable;

        public void Call(object sender)
        {
            OnPlayerFoundCollectable?.Invoke(sender, EventArgs.Empty);
        }
    } 
}