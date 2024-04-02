using System;

namespace Character.Michael.Events
{
    public class HarvestingPlantEvent
    {
        public event Action<bool> OnMichaelHarvestingPlant;

        public void Call(bool isHarvesting)
        {
            OnMichaelHarvestingPlant?.Invoke(isHarvesting);
        }
    }
}