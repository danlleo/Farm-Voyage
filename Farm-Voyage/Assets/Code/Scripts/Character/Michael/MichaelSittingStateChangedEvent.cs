using System;
using UnityEngine;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public class MichaelSittingStateChangedEvent : MonoBehaviour
    {
        public event Action<bool> OnMichaelSittingStateChanged;

        public void Call(bool isSitting)
        {
	        OnMichaelSittingStateChanged?.Invoke(isSitting);
        }
    }
}