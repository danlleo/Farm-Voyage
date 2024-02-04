using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerIdleEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerIdle;

        public void Call(object sender)
        {
            OnPlayerIdle?.Invoke(sender, EventArgs.Empty);
        }
    }
}