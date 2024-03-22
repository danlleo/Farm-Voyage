using Farm.Plants;
using UnityEngine;

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
            _michael.WaterStateObserver.OnPlantWateringStateChanged += WaterStateObserver_OnPlantWateringStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _michael.WaterStateObserver.OnPlantWateringStateChanged -= WaterStateObserver_OnPlantWateringStateChanged;
        }
        
        public override void OnEnter()
        {
            _michael.MichaelLocomotion.HandleMoveDestination(_plant.transform, () =>
            {
                Debug.Log("Finished moving");
            });
        }
        
        private void HandleWateredPlant(Plant plant)
        {
            if (!ReferenceEquals(_plant, plant)) return;
            
            _michael.MichaelLocomotion.StopAllMovement();
            _stateMachine.ChangeState(_michael.StateFactory.Idle());
        }
        
        private void WaterStateObserver_OnPlantWateringStateChanged(Plant plant, bool needsWatering)
        {
            HandleWateredPlant(plant);
        }
    }
}