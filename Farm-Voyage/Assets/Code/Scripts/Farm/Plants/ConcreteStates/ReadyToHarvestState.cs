using Character;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
        private readonly PlantHarvestingVisitor _plantHarvestingVisitor;
        
        public ReadyToHarvestState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plantHarvestingVisitor = new PlantHarvestingVisitor(plant);
        }
        
        public override void OnInteracted(ICharacter initiator)
        {
            initiator.Accept(_plantHarvestingVisitor);
        }
    }
}