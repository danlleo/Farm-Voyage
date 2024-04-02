using System;

namespace Character.Player.Events
{
    public class WateringStateChangedEvent
    {
        public event Action<bool> OnPlayerWateringStateChanged;

        public void Call(bool isWatering)
        {
            OnPlayerWateringStateChanged?.Invoke(isWatering);
        }
    }
}