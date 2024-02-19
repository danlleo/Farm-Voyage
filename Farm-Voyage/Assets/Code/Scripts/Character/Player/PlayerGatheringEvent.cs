using System;
using Farm;
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
        public readonly float GatheringSpeed;
        public ResourceType ResourceType;
        
        public PlayerGatheringEventArgs(bool isGathering, ResourceType resourceType, float gatheringSpeed = 1f)
        {
            IsGathering = isGathering;
            ResourceType = resourceType;
            GatheringSpeed = gatheringSpeed;
        }
    }
}
