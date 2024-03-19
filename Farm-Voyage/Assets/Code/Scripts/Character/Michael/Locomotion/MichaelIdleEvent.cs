using System;
using UnityEngine;

namespace Character.Michael.Locomotion
{
    [DisallowMultipleComponent]
    public class MichaelIdleEvent : MonoBehaviour
    {
        public event Action OnMichaelIdle;

        public void Call()
        {
            OnMichaelIdle?.Invoke();
        }
    }
}