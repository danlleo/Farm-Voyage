using Character.Player;
using Common;

namespace Farm.Plants.ConcreteStates
{
    public class ReadyToHarvestState : State
    {
        private readonly Harvester _harvester;
        
        public ReadyToHarvestState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _harvester = new Harvester(plant);
        }
        
        public override void OnInteracted(IVisitable initiator)
        {
            initiator.Accept(_harvester);
        }

        public override void OnPlayerStoppedInteracting(Player player)
        {
            player.Events.HarvestingStateChangedEvent.Call(false);
        }
    }
}