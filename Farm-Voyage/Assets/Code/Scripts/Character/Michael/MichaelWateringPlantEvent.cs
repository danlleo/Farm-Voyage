using System;
using UnityEngine;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public class MichaelWateringPlantEvent : MonoBehaviour
    {
        public event Action<bool> OnMichaelWateringPlant;

        public void Call(bool isWatering)
        {
            OnMichaelWateringPlant?.Invoke(isWatering);
        }
    }
}