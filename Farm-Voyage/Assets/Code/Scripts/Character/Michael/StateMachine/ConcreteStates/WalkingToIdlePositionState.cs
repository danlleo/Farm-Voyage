using Farm.Plants;
using UnityEngine;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class WalkingToIdlePositionState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        
        public WalkingToIdlePositionState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
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

        public override void OnEnter()
        {
            _michael.MichaelLocomotion.HandleMoveDestination(Vector3.zero,
                () => _stateMachine.ChangeState(_michael.StateFactory.Idle()));
        }
        
        private void WaterStateObserver_OnPlantWateringStateChanged(Plant plant, bool needsWatering)
        {
            if (!needsWatering) return;
            
            _michael.MichaelLocomotion.StopAllMovement();
            _stateMachine.ChangeState(_michael.StateFactory.WalkingToPlant(plant));
        }
    }
}
