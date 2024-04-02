using System;
using UnityEngine;

namespace Character.Player.Events
{
    public class UsingWorkbenchEvent
    {
        public event EventHandler<PlayerUsingWorkbenchEventArgs> OnPlayerUsingWorkbench;

        public void Call(object sender, PlayerUsingWorkbenchEventArgs playerUsingWorkbenchEventArgs)
        {
            OnPlayerUsingWorkbench?.Invoke(sender, playerUsingWorkbenchEventArgs);
        }
    }

    public class PlayerUsingWorkbenchEventArgs : EventArgs
    {
        public readonly bool IsUsingWorkbench;

        public PlayerUsingWorkbenchEventArgs(bool isUsingWorkbench)
        {
            IsUsingWorkbench = isUsingWorkbench;
        }
    }
}