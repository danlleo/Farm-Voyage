using System.Collections;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
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

        public override void OnInteracted()
        {
            _plant.Harvest();
        }

        private void HarvestWithDelay()
        {
            if (_delayHarvestingRoutine != null)
                _delayHarvestingRoutine = _plant.StartCoroutine(DelayHarvestingRoutine());
        }

        private IEnumerator DelayHarvestingRoutine()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}