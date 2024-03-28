using System;
using UnityEngine;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public class MichaelPerformingGardeningActionEvent : MonoBehaviour
    {
        public event Action<GardeningActionType, Action> OnMichaelPerformingGardeningAction;

        public void Call(GardeningActionType gardeningActionType, Action onFinishedGardeningAction)
        {
            OnMichaelPerformingGardeningAction?.Invoke(gardeningActionType, onFinishedGardeningAction);
        }
    }
}