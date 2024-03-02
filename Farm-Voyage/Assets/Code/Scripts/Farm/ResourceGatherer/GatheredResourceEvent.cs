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
        public readonly int TimesInteractedAmount;
        public readonly int InteractAmountToDestroy;

        public GatheredResourceEventArgs(int gatheredQuantity, int timesInteractedAmount, int interactAmountToDestroy)
        {
            GatheredQuantity = gatheredQuantity;
            TimesInteractedAmount = timesInteractedAmount;
            InteractAmountToDestroy = interactAmountToDestroy;
        }
    }
}