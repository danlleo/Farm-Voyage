using Farm.Plants.ConcreteStates;

namespace Farm.Plants
{
    public class StateFactory
    {
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;

        public StateFactory(Plant plant, StateMachine stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
        }

        public State Growing()
        {
            return new GrowingState(_plant, _stateMachine);
        }

        public State Watering()
        {
            return new WateringState(_plant, _stateMachine);
        }

        public State ReadyToHarvest()
        {
            return new ReadyToHarvestState(_plant, _stateMachine);
        }
    }
}