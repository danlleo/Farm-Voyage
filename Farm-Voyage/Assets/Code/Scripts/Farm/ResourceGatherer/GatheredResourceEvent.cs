using System;
using System.Collections.Generic;
using UnityEngine;

namespace Farm.ResourceGatherer
{
    [DisallowMultipleComponent]
    public class GatheredResourceEvent : MonoBehaviour
    {
        public event EventHandler<GatheredResourceEventArgs> OnGatheredResource;

        public void Call(object sender, GatheredResourceEventArgs gatheredResourceEventArgs)
        {
            OnGatheredResource?.Invoke(sender, gatheredResourceEventArgs);
        }
    }

    public class GatheredResourceEventArgs : EventArgs
    {
        public readonly int GatheredQuantity;
        public readonly List<Material> AllMaterials;

        public GatheredResourceEventArgs(int gatheredQuantity, List<Material> allMaterials)
        {
            GatheredQuantity = gatheredQuantity;
            AllMaterials = allMaterials;
        }
    }
}