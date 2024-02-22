using System;
using Farm.FarmResources;
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
        public readonly bool HasFullyGathered;
        public readonly float GatheringSpeed;
        public readonly ResourceType ResourceType;
        public readonly Transform LockedTransform;

        public PlayerGatheringEventArgs(bool isGathering, bool hasFullyGathered, ResourceType resourceType,
            Transform lockedTransform, float gatheringSpeed = 1f)
        {
            IsGathering = isGathering;
            HasFullyGathered = hasFullyGathered;
            ResourceType = resourceType;
            LockedTransform = lockedTransform;
            GatheringSpeed = gatheringSpeed;
        }
    }
}
