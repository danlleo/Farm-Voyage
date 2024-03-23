using System;
using System.Collections;
using Character;
using UnityEngine;
using Utilities;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
        public static event Action<Plant> OnAnyPlantHarvested; 
        
        private readonly Plant _plant;
        private Coroutine _delayHarvestingRoutine;
        
        public ReadyToHarvestState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
        }

        public override void OnEnter()
        {
            Debug.Log("Ready to harvest");
        }

        public override void OnInteracted(ICharacter initiator)
        {
            DelayHarvest();
        }

        public override void OnStoppedInteracting()
        {
            _plant.StopCoroutine(_delayHarvestingRoutine);
            _delayHarvestingRoutine = null;
        }

        private void DelayHarvest()
        {
            _delayHarvestingRoutine ??= _plant.StartCoroutine(DelayHarvestingRoutine());
        }
        
        private void Harvest()
        {
            _plant.GetComponent<BoxCollider>().Disable();
            _plant.PlantArea.ClearPlantArea();
            OnAnyPlantHarvested?.Invoke(_plant);
        }
        
        private IEnumerator DelayHarvestingRoutine()
        {
            yield return new WaitForSeconds(1f);
            Harvest();
        }
    }
}