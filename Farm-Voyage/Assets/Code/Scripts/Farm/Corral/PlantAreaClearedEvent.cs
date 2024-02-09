using System;
using Farm.Plants;
using UnityEngine;

namespace Farm.Corral
{
    [DisallowMultipleComponent]
    public class PlantAreaClearedEvent : MonoBehaviour
    {
        public event EventHandler<PlantAreaClearedEventArgs> OnPlantAreaCleared;

        public void Call(object sender, PlantAreaClearedEventArgs plantAreaClearedEventArgs)
        {
            OnPlantAreaCleared?.Invoke(sender, plantAreaClearedEventArgs);
        }
    }

    public class PlantAreaClearedEventArgs : EventArgs
    {
        public readonly Plant Plant;

        public PlantAreaClearedEventArgs(Plant plant)
        {
            Plant = plant;
        }
    }
}