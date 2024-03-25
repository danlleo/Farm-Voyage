﻿using Farm.Corral;
using Farm.Plants;
using UnityEngine;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class HarvestingState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        private readonly Plant _plant;
        
        public HarvestingState(Michael michael, StateMachine stateMachine, Plant plant) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
            _plant = plant;
        }

        public override void SubscribeToEvents()
        {
            PlantArea.OnAnyPlantHarvested += PlantArea_OnAnyPlantHarvested;
        }

        public override void UnsubscribeFromEvents()
        {
            PlantArea.OnAnyPlantHarvested -= PlantArea_OnAnyPlantHarvested;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered HarvestingState State");
            _michael.MichaelHarvestingPlantEvent.Call(true);
        }

        public override void OnExit()
        {
            _michael.MichaelHarvestingPlantEvent.Call(false);
        }

        public override void Tick()
        {
            _plant.Interact(_michael);
        }

        private void PlantArea_OnAnyPlantHarvested(Plant plant)
        {
            if (ReferenceEquals(_plant, plant))
            {
                _stateMachine.ChangeState(_michael.StateFactory.EvaluateWateringNeeds());
            }
        }
    }
}