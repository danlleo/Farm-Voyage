using System;
using Character.Player;
using Common;
using Sound;

namespace Farm.Plants.ConcreteStates
{
    public class WateringState : State
    {
        public static event Action<Plant, bool> OnAnyWateringStateChanged;
        
        private readonly Plant _plant;
        private readonly StateMachine _stateMachine;
        private readonly Waterer _waterer;
        
        public WateringState(Plant plant, StateMachine stateMachine) : base(plant, stateMachine)
        {
            _plant = plant;
            _stateMachine = stateMachine;
            _waterer = new Waterer(_plant, _plant.PlayerInventory);
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
            SoundFXManager.Instance.PlayRandomSoundFX3DClip(_plant.WateringAudioClips, _plant.transform, 0.3f);
            OnAnyWateringStateChanged?.Invoke(_plant, true);
        }

        public override void OnExit()
        {
            _plant.PlantArea.ProgressIcon.ResumeProgress(_plant.PlantArea);
        }

        public override void OnInteracted(IVisitable initiator)
        {
            initiator.Accept(_waterer);
        }

        public override void OnPlayerStoppedInteracting(Player player)
        {
            player.Events.WateringStateChangedEvent.Call(false);
        }

        private void Plant_OnPlantFinishedWatering()
        {
            OnAnyWateringStateChanged?.Invoke(_plant, false);
            _stateMachine.ChangeState(_plant.StateFactory.Growing());
        }
    }
}