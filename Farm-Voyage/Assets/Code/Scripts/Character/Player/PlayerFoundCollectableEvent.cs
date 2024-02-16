using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerFoundCollectableEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerFoundCollectable;

        public void Call(object sender)
        {
            OnPlayerFoundCollectable?.Invoke(sender, EventArgs.Empty);
        }
    } 
}