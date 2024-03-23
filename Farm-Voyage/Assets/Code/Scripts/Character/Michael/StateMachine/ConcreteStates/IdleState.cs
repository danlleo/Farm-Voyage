using Farm.Plants;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class IdleState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        
        public IdleState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _michael.WaterStateObserver.OnPlantWateringStateChanged += WaterStateObserver_OnPlantWateringStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _michael.WaterStateObserver.OnPlantWateringStateChanged -= WaterStateObserver_OnPlantWateringStateChanged;
        }

        private void ProceedToWalkIfPlantNeedsWatering(Farm.Plants.Plant plant, bool needsWatering)
        {
            if (!needsWatering) return;
            
            _stateMachine.ChangeState(_michael.StateFactory.WalkingToPlant(plant));
        }
        
        private void WaterStateObserver_OnPlantWateringStateChanged(Farm.Plants.Plant plant, bool needsWatering)
        {
            ProceedToWalkIfPlantNeedsWatering(plant, needsWatering);
        }
    }
}