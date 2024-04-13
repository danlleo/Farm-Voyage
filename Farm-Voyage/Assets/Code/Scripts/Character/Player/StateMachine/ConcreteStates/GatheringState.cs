using Character.Player.Events;
using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class GatheringState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        private readonly Transform _resourceGathererTransform;
        
        public GatheringState(Player player, StateMachine stateMachine, Transform resourceGathererTransform) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
            _resourceGathererTransform = resourceGathererTransform;
        }

        public override void SubscribeToEvents()
        {
            _player.Events.GatheringEvent.OnPlayerGathering += OnGathering;
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged +=
                Player_OnExtractingWaterStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.GatheringEvent.OnPlayerGathering -= OnGathering;
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged -=
                Player_OnExtractingWaterStateChanged;
        }

        public override void OnEnter()
        {
            _player.Locomotion.StartStickRotation(_resourceGathererTransform, () =>
            {
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
            });
        }

        public override void Tick()
        {
            _player.Interact.TryInteract();
            _player.Locomotion.HandleGroundedMovement();
        }
        
        private void OnGathering(object sender, PlayerGatheringEventArgs e)
        {
            if (!e.HasFullyGathered) return;
            
            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
        
        private void Player_OnExtractingWaterStateChanged(bool isExtractingWater)
        {
            if (!isExtractingWater) return;
            
            _stateMachine.ChangeState(_player.StateFactory.ExtractingWater());
        }
    }
}