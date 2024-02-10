using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerStartedCarryingEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerStartedCarrying;

        public void Call(object sender)
        {
            OnPlayerStartedCarrying?.Invoke(sender, EventArgs.Empty);
        }
    }
}