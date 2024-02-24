using System;
using UnityEngine;

namespace Character.Player.Locomotion
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