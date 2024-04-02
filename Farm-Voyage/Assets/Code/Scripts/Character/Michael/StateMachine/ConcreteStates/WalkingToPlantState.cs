using Farm.Plants;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class WalkingToPlantState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        private readonly Plant _plant;
        
        public WalkingToPlantState(Michael michael, StateMachine stateMachine, Plant plant) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
            _plant = plant;
        }

        public override void SubscribeToEvents()
        {
            _plant.PlantFinishedWateringEvent.OnPlantFinishedWatering += PlantFinishedWateringEvent_OnPlantFinishedWatering;
        }

        public override void UnsubscribeFromEvents()
        {
            _plant.PlantFinishedWateringEvent.OnPlantFinishedWatering -= PlantFinishedWateringEvent_OnPlantFinishedWatering;
        }

        public override void OnEnter()
        {
            _michael.Locomotion.HandleMoveDestination(_plant.transform, () =>
            {
                _stateMachine.ChangeState(_michael.StateFactory.Watering(_plant));
            });
        }
        
        private void PlantFinishedWateringEvent_OnPlantFinishedWatering()
        {
            _michael.Locomotion.StopAllMovement();
            _stateMachine.ChangeState(_michael.StateFactory.EvaluateWateringNeeds());
        }
    }
}