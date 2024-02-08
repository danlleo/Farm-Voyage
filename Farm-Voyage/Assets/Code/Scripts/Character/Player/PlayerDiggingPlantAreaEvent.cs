using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerDiggingPlantAreaEvent : MonoBehaviour
    {
        public event EventHandler<PlayerDiggingPlantAreaEventArgs> OnPlayerDiggingPlantArea;

        public void Call(object sender, PlayerDiggingPlantAreaEventArgs playerDiggingPlantAreaEventArgs) {
            OnPlayerDiggingPlantArea?.Invoke(sender, playerDiggingPlantAreaEventArgs);
        }
    }

    public class PlayerDiggingPlantAreaEventArgs : EventArgs
    {
        public readonly bool IsDigging;

        public PlayerDiggingPlantAreaEventArgs(bool isDigging)
        {
            IsDigging = isDigging;
        }
    }
}