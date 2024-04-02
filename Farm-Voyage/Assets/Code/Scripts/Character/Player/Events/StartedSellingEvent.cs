using System;

namespace Character.Player.Events
{
    public class StartedSellingEvent
    {
        public event Action OnPlayerStartedSelling;

        public void Call()
        {
            OnPlayerStartedSelling?.Invoke();
        }
    }
}