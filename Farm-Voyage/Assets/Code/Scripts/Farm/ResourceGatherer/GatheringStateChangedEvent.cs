using System;
using UnityEngine;

namespace Farm.ResourceGatherer
{
    [DisallowMultipleComponent]
    public class GatheringStateChangedEvent : MonoBehaviour
    {
        public event EventHandler<GatheringStateChangedEventArgs> OnGatheringStateChanged;

        public void Call(object sender, GatheringStateChangedEventArgs gatheringStateChangedEventArgs)
        {
            OnGatheringStateChanged?.Invoke(sender, gatheringStateChangedEventArgs);
        }
    }

    public class GatheringStateChangedEventArgs : EventArgs
    {
        public readonly bool IsGathering;

        public GatheringStateChangedEventArgs(bool isGathering)
        {
            IsGathering = isGathering;
        }
    }
}