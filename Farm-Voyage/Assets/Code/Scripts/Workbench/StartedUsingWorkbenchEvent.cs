using System;
using UnityEngine;

namespace Workbench
{
    [DisallowMultipleComponent]
    public class StartedUsingWorkbenchEvent : MonoBehaviour
    {
        public event EventHandler OnStartedUsingWorkbench;

        public void Call(object sender)
        {
            OnStartedUsingWorkbench?.Invoke(sender, EventArgs.Empty);
        }
    }
}