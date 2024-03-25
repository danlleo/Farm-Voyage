using Farm.Plants;
using UnityEngine;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class EvaluateWateringNeedsState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        
        public EvaluateWateringNeedsState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered EvaluateWatering State");
            EvaluateWateringNeeds();
        }

        private void EvaluateWateringNeeds()
        {
            if (_michael.WaterStateObserver.TryGetPlantToWater(out Plant plant))
            {
                _stateMachine.ChangeState(_michael.StateFactory.WalkingToPlant(plant));
                return;
            }
            
            _stateMachine.ChangeState(_michael.StateFactory.WalkingToIdlePosition());
        }
    }
}