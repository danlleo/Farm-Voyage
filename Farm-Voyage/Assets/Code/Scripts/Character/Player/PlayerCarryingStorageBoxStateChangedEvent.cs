using System;
using Farm.Corral;
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
        public readonly StorageBox StorageBox;
        public readonly bool IsCarrying;

        public PlayerCarryingStorageBoxStateChangedEventArgs(StorageBox storageBox, bool isCarrying)
        {
            StorageBox = storageBox;
            IsCarrying = isCarrying;
        }
    }
}