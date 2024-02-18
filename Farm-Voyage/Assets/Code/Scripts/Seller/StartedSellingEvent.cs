using System;
using UnityEngine;

namespace Seller
{
    [DisallowMultipleComponent]
    public class StartedSellingEvent : MonoBehaviour
    {
        public event EventHandler OnStartedSelling;

        public void Call(object sender)
        {
            OnStartedSelling?.Invoke(sender, EventArgs.Empty);
        }
    }
}