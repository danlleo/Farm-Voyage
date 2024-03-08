using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerEnteringHomeEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerEnteringHome;

        public void Call(object sender)
        {
            OnPlayerEnteringHome?.Invoke(sender, EventArgs.Empty);
        }
    }
}