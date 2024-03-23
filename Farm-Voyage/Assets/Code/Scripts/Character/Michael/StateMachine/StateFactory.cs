using Character.Michael.StateMachine.ConcreteStates;
using Farm.Plants;

namespace Character.Michael.StateMachine
{
    public class StateFactory
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;

        public StateFactory(Michael michael, StateMachine stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public State Idle()
        {
            return new IdleState(_michael, _stateMachine);
        }
        
        public State Gardening()
        {
            return new GardeningState(_michael, _stateMachine);
        }

        public State WalkingToPlant(Farm.Plants.Plant plant)
        {
            return new WalkingToPlantState(_michael, _stateMachine, plant);
        }

        public State Watering(Farm.Plants.Plant plant)
        {
            return new WateringState(_michael, _stateMachine, plant);
        }
    }
}