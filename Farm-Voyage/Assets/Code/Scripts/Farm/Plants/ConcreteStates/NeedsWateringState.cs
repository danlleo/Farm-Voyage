using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class NeedsWateringState : State
    {
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;
        
        public NeedsWateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            Debug.Log("Needs watering");
        }

        public override void OnInteracted()
        {
            // TODO: Water plant in here
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}