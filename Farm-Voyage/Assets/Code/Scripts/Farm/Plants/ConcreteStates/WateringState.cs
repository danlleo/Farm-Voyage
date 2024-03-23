using System;
using Character;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class WateringState : State
    {
        public static event Action<Plant, bool> OnAnyWateringStateChanged;
        
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;
        
        public WateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
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
            OnAnyWateringStateChanged?.Invoke(_plant, true);
            Debug.Log("Needs watering");
        }

        public override void OnInteracted(ICharacter initiator)
        {
            // TODO: Finish logic later.
        }
        
        private void Plant_OnPlantFinishedWatering()
        {
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}