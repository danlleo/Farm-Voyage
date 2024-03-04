using System.Collections;
using Farm.Tool.ConcreteTools;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class NeedsWateringState : State
    {
        private readonly float _timeToWaterInSeconds = 2f;
        
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;

        private Coroutine _wateringRoutine;

        private WaterCan _waterCan;
        
        public NeedsWateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;

            plant.PlayerInventory.TryGetTool(out _waterCan);
        }

        public override void OnEnter()
        {
            Debug.Log("Needs watering");
        }

        public override void OnInteracted()
        {
            if (_waterCan == null) return;
            if (!_waterCan.HasWaterLeft()) return;
            
            _wateringRoutine ??= _plant.StartCoroutine(WateringRoutine());
        }

        public override void OnStoppedInteracting()
        {
            Debug.Log("Stopped watering");
            
            if (_wateringRoutine != null)
                _plant.StopCoroutine(_wateringRoutine);
        }

        private IEnumerator WateringRoutine()
        {
            yield return new WaitForSeconds(_timeToWaterInSeconds);
            
            _waterCan.EmptyCan();
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}