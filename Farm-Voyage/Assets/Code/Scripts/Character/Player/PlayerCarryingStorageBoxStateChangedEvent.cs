using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerCarryingStorageBoxStateChangedEvent : MonoBehaviour
    {
        public event EventHandler<PlayerCarryingStorageBoxStateChangedEventArgs> OnPlayerCarryingStorageBoxStateChanged;

        public void Call(object sender, PlayerCarryingStorageBoxStateChangedEventArgs playerCarryingStorageBoxStateChangedEventArgs)
        {
            OnPlayerCarryingStorageBoxStateChanged?.Invoke(sender, playerCarryingStorageBoxStateChangedEventArgs);
        }
    }

    public class PlayerCarryingStorageBoxStateChangedEventArgs : EventArgs
    {
        public readonly bool IsCarrying;

        public PlayerCarryingStorageBoxStateChangedEventArgs(bool isCarrying)
        {
            IsCarrying = isCarrying;
        }
    }
}