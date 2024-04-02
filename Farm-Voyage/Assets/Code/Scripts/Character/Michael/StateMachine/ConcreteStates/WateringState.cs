using Farm.Plants;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class WateringState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        private readonly Plant _plant;
        
        public WateringState(Michael michael, StateMachine stateMachine, Plant plant) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
            _plant = plant;
        }

        public override void SubscribeToEvents()
        {
            _plant.PlantFinishedWateringEvent.OnPlantFinishedWatering += Plant_OnPlantFinishedWatering;
        }

        public override void UnsubscribeFromEvents()
        {
            _plant.PlantFinishedWateringEvent.OnPlantFinishedWatering -= Plant_OnPlantFinishedWatering;
        }
        
        public override void OnEnter()
        {
            _michael.Events.MichaelWateringPlantEvent.Call(true);
        }

        public override void OnExit()
        {
            _michael.Events.MichaelWateringPlantEvent.Call(false);
        }

        public override void Tick()
        {
            _plant.Interact(_michael);
        }
        
        private void Plant_OnPlantFinishedWatering()
        {
            _stateMachine.ChangeState(_michael.StateFactory.Harvesting(_plant));
        }
    }
}