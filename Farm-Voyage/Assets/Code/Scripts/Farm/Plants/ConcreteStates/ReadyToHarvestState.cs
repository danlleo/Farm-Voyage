using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
        private readonly Plant _plant;
        
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
            // TODO: Harvest here
            _plant.Harvest();
        }
    }
}