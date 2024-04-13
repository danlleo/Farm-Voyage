using System;

namespace Character.Player.Events
{
    public class ExtractingWaterStateChangedEvent
    {
        public event Action<bool> OnPlayerExtractingWaterStateChanged;

        public void Call(bool isExtractingWater)
        {
            OnPlayerExtractingWaterStateChanged?.Invoke(isExtractingWater);
        }
    }
}