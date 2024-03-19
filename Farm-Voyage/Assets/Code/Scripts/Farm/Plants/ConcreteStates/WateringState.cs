using System;
using System.Collections;
using Farm.Tool.ConcreteTools;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class WateringState : State
    {
        public static event Action<Plant, bool> OnAnyWateringStateChanged;
        
        private const float TimeToWaterInSeconds = 2f;

        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;

        private readonly WaterCan _waterCan;

        private Coroutine _wateringRoutine;
        
        public WateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;

            plant.PlayerInventory.TryGetTool(out _waterCan);
        }

        public override void OnEnter()
        {
            OnAnyWateringStateChanged?.Invoke(_plant, true);
            Debug.Log("Needs watering");
        }

        public override void OnInteracted()
        {
            if (_waterCan == null) return;
            if (!(_waterCan.CurrentWaterCapacityAmount > 0)) return;
            
            _wateringRoutine ??= _plant.StartCoroutine(WateringRoutine());
        }

        public override void OnStoppedInteracting()
        {
            Debug.Log("Stopped watering");

            if (_wateringRoutine == null) return;
            
            _plant.StopCoroutine(_wateringRoutine);
            _wateringRoutine = null;
        }

        private IEnumerator WateringRoutine()
        {
            yield return new WaitForSeconds(TimeToWaterInSeconds);
            
            _waterCan.EmptyCan();
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}