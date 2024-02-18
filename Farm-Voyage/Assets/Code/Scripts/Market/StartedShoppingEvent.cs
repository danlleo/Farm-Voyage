using System;
using UnityEngine;

namespace Market
{
    [DisallowMultipleComponent]
    public class StartedShoppingEvent : MonoBehaviour
    {
        public event EventHandler OnStartedShopping;

        public void Call(object sender)
        {
            OnStartedShopping?.Invoke(sender, EventArgs.Empty);
        }
    }
}