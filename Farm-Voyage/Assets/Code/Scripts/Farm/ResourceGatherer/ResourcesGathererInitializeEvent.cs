using System;
using UnityEngine;

namespace Farm.ResourceGatherer
{
    [DisallowMultipleComponent]
    public class ResourcesGathererInitializeEvent : MonoBehaviour
    {
        public event EventHandler<ResourcesGathererInitializeEventArgs> OnResourcesGathererInitialize;

        public void Call(object sender, ResourcesGathererInitializeEventArgs resourcesGathererInitializeEventArgs)
        {
            OnResourcesGathererInitialize?.Invoke(sender, resourcesGathererInitializeEventArgs);
        }
    }

    public class ResourcesGathererInitializeEventArgs : EventArgs
    {
        public readonly GameObject VisualGameObject;

        public ResourcesGathererInitializeEventArgs(GameObject visualGameObject)
        {
            VisualGameObject = visualGameObject;
        }
    }
}