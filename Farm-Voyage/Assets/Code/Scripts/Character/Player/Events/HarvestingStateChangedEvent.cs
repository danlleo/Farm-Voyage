using System;

namespace Character.Player.Events
{
    public class HarvestingStateChangedEvent
    {
        public event Action<bool> OnPlayerHarvestingStateChanged;

        public void Call(bool isHarvesting)
        {
            OnPlayerHarvestingStateChanged?.Invoke(isHarvesting);
        }
    }
}