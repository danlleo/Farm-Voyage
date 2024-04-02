using Character;
using Character.Player;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
        private readonly PlantHarvestingVisitor _plantHarvestingVisitor;
        
        public ReadyToHarvestState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plantHarvestingVisitor = new PlantHarvestingVisitor(plant);
        }
        
        public override void OnInteracted()
        {
            // initiator.Accept(_plantHarvestingVisitor);
        }

        public override void OnStoppedInteracting(Player player)
        {
            player.PlayerEvents.PlayerHarvestingStateChangedEvent.Call(false);
        }
    }
}