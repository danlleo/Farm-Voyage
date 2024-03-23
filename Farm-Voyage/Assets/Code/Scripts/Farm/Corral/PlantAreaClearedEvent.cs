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
        public readonly Plants.Plant Plant;

        public PlantAreaClearedEventArgs(Plants.Plant plant)
        {
            Plant = plant;
        }
    }
}