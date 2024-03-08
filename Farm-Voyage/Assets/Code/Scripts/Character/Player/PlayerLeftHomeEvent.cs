using System;
using UnityEngine;

namespace Character.Player
{
    [DisallowMultipleComponent]
    public class PlayerLeftHomeEvent : MonoBehaviour
    {
        public event EventHandler OnPlayerLeftHome;

        public void Call(object sender)
        {
            OnPlayerLeftHome?.Invoke(sender, EventArgs.Empty);
        }
    }
}