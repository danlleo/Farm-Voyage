using System;
using UnityEngine;

namespace Character.Michael.Locomotion
{
    [DisallowMultipleComponent]
    public class MichaelWalkingEvent : MonoBehaviour
    {
        public event Action OnMichaelWalking;

        public void Call()
        {
            OnMichaelWalking?.Invoke();
        }
    }
}