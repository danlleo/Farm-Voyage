using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerExtractingWaterEvent : MonoBehaviour
    {
        public event EventHandler<PlayerExtractingWaterEventArgs> OnPlayerExtractingWater;

        public void Call(object sender, PlayerExtractingWaterEventArgs playerExtractingWaterEventArgs)
        {
            OnPlayerExtractingWater?.Invoke(sender, playerExtractingWaterEventArgs);
        }
    }

    public class PlayerExtractingWaterEventArgs : EventArgs
    {
        public readonly bool IsExtracting;

        public PlayerExtractingWaterEventArgs(bool isExtracting)
        {
            IsExtracting = isExtracting;
        }
    }
}