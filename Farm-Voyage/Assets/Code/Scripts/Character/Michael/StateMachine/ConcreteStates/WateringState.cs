using Farm.Plants;
using UnityEngine;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class WateringState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        private readonly Farm.Plants.Plant _plant;
        
        public WateringState(Michael michael, StateMachine stateMachine, Farm.Plants.Plant plant) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
            _plant = plant;
        }

        public override void OnEnter()
        {
            
        }
    }
}