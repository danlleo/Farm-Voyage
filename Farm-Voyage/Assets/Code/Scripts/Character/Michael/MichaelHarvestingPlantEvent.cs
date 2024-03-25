using System;
using UnityEngine;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public class MichaelHarvestingPlantEvent : MonoBehaviour
    {
        public event Action<bool> OnMichaelHarvestingPlant;

        public void Call(bool isHarvesting)
        {
            OnMichaelHarvestingPlant?.Invoke(isHarvesting);
        }
    }
}