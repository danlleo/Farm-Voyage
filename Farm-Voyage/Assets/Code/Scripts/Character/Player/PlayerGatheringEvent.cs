using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerGatheringEvent : MonoBehaviour
    {
        public event EventHandler<PlayerGatheringEventArgs> OnPlayerGathering;

        public void Call(object sender, PlayerGatheringEventArgs playerGatheringEventArgs)
        {
            OnPlayerGathering?.Invoke(sender, playerGatheringEventArgs);
        }
    }

    public class PlayerGatheringEventArgs : EventArgs
    {
        public readonly bool IsGathering;

        public PlayerGatheringEventArgs(bool isGathering)
        {
            IsGathering = isGathering;
        }
    }
}
