using System;
using UnityEngine;

namespace Farm.Plants
{
    [DisallowMultipleComponent]
    public class PlantFinishedWateringEvent : MonoBehaviour
    {
        public event Action OnPlantFinishedWatering;

        public void Call()
        {
            OnPlantFinishedWatering?.Invoke();
        }
    }
}