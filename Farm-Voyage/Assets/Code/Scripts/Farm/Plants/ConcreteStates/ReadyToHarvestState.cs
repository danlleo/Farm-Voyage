using Character;
using UnityEngine;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
        private readonly PlantHarvestingVisitor _plantHarvestingVisitor;
        
        private Coroutine _delayHarvestingRoutine;
        
        public ReadyToHarvestState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plantHarvestingVisitor = new PlantHarvestingVisitor(plant);
        }

        public override void OnEnter()
        {
            Debug.Log("Ready to harvest");
        }

        public override void OnInteracted(ICharacter initiator)
        {
            initiator.Accept(_plantHarvestingVisitor);
        }
    }
}