using System;

namespace Character.Michael.Events
{
    public class PerformingGardeningActionEvent
    {
        public event Action<GardeningActionType, Action> OnMichaelPerformingGardeningAction;

        public void Call(GardeningActionType gardeningActionType, Action onFinishedGardeningAction)
        {
            OnMichaelPerformingGardeningAction?.Invoke(gardeningActionType, onFinishedGardeningAction);
        }
    }
}