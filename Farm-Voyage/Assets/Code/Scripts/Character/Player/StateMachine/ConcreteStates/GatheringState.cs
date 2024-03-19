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
            _player.PlayerGatheringEvent.OnPlayerGathering += Player_OnPlayerGathering;
            _player.PlayerExtractingWaterEvent.OnPlayerExtractingWater += Player_OnPlayerExtractingWater;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerGatheringEvent.OnPlayerGathering -= Player_OnPlayerGathering;
            _player.PlayerExtractingWaterEvent.OnPlayerExtractingWater -= Player_OnPlayerExtractingWater;
        }

        public override void OnEnter()
        {
            _player.PlayerLocomotion.HandleStickRotation(_resourceGathererTransform, () =>
            {
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
            });
        }

        public override void Tick()
        {
            _player.PlayerInteract.TryInteract();
            _player.PlayerLocomotion.HandleGroundedMovement();
        }
        
        private void Player_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            if (!e.HasFullyGathered) return;
            
            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
        
        private void Player_OnPlayerExtractingWater(object sender, PlayerExtractingWaterEventArgs e)
        {
            if (!e.IsExtracting) return;
            
            _stateMachine.ChangeState(_player.StateFactory.ExtractingWater());
        }
    }
}