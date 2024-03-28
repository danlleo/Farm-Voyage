using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerWateringStateChangedEvent : MonoBehaviour
    {
        public event Action<bool> OnPlayerWateringStateChanged;

        public void Call(bool isWatering)
        {
            OnPlayerWateringStateChanged?.Invoke(isWatering);
        }
    }
}