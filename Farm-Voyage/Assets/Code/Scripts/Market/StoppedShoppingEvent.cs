using System;
using UnityEngine;

namespace Market
{
    [DisallowMultipleComponent]
    public class StoppedShoppingEvent : MonoBehaviour
    {
        public event EventHandler OnStoppedShopping;

        public void Call(object sender)
        {
            OnStoppedShopping?.Invoke(sender, EventArgs.Empty);
        }
    }
}