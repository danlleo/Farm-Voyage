using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerShoppingEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerShopping;

        public void Call(object sender)
        {
            OnPlayerShopping?.Invoke(sender, EventArgs.Empty);
        }
    }
}