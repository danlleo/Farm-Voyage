using System;
using Character;
using Character.Player;

namespace Farm.Plants.ConcreteStates
{
    public class WateringState : State
    {
        public static event Action<Plant, bool> OnAnyWateringStateChanged;
        
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;
        private readonly PlantWateringVisitor _plantWateringVisitor;
        
        public WateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
            _plantWateringVisitor = new PlantWateringVisitor(_plant, _plant.PlayerInventory);
        }

        public override void SubscribeToEvents()
        {
            _plant.PlantFinishedWateringEvent.OnPlantFinishedWatering += Plant_OnPlantFinishedWatering;
        }

        public override void UnsubscribeFromEvents()
        {
            _plant.PlantFinishedWateringEvent.OnPlantFinishedWatering -= Plant_OnPlantFinishedWatering;
        }

        public override void OnEnter()
        {
            OnAnyWateringStateChanged?.Invoke(_plant, true);
        }

        public override void OnInteracted(ICharacter initiator)
        {
            initiator.Accept(_plantWateringVisitor);
        }

        public override void OnStoppedInteracting(Player player)
        {
            // TODO: consider making it more clean
            player.PlayerEvents.PlayerWateringStateChangedEvent.Call(false);
        }

        private void Plant_OnPlantFinishedWatering()
        {
            OnAnyWateringStateChanged?.Invoke(_plant, false);
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}