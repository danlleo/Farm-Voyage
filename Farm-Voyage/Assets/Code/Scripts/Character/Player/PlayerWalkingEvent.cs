using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerWalkingEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerWalking;

        public void Call(object sender)
        {
            OnPlayerWalking?.Invoke(sender, EventArgs.Empty);
        }
    }
}