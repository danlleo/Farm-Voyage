using System.Collections;
using Farm.Tool.ConcreteTools;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class NeedsWateringState : State
    {
        private const float TimeToWaterInSeconds = 2f;

        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;

        private readonly WaterCan _waterCan;

        private Coroutine _wateringRoutine;
        
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
            if (!(_waterCan.CurrentWaterCapacityAmount > 0)) return;
            
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
            yield return new WaitForSeconds(TimeToWaterInSeconds);
            
            _waterCan.EmptyCan();
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}