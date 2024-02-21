using System;
using UnityEngine;

namespace Farm.ResourceGatherer
{
    [DisallowMultipleComponent]
    public class FullyGatheredEvent : MonoBehaviour
    {
        public event EventHandler OnFullyGathered;

        public void Call(object sender)
        {
            OnFullyGathered?.Invoke(sender, EventArgs.Empty);           
        }
    }
}