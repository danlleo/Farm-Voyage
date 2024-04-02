using System;

namespace Character.Michael.Events
{
    public class MichaelWateringPlantEvent
    {
        public event Action<bool> OnMichaelWateringPlant;

        public void Call(bool isWatering)
        {
            OnMichaelWateringPlant?.Invoke(isWatering);
        }
    }
}