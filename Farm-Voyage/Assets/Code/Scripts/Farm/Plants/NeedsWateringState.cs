using UnityEngine;

namespace Farm.Plants
{
    public class NeedsWateringState : State
    {
        private Plant _plant;
        private StateMachine _stateMachine;
        
        public NeedsWateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            Debug.Log("Needs watering");
        }
    }
}