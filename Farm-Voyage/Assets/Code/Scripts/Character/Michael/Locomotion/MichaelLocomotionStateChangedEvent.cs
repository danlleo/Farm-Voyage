using System;
using UnityEngine;

namespace Character.Michael.Locomotion
{
    [DisallowMultipleComponent]
    public class MichaelLocomotionStateChangedEvent : MonoBehaviour
    {
        public event Action<bool> OnMichaelWalking;

        public void Call(bool isWalking)
        {
            OnMichaelWalking?.Invoke(isWalking);
        }
    }
}