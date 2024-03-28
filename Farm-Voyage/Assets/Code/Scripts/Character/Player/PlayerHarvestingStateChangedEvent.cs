using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerHarvestingStateChangedEvent : MonoBehaviour
    {
        public event Action<bool> OnPlayerHarvestingStateChanged;

        public void Call(bool isHarvesting)
        {
            OnPlayerHarvestingStateChanged?.Invoke(isHarvesting);
        }
    }
}