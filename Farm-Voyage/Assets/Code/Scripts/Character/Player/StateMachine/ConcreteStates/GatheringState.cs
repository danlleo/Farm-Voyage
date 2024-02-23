using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class GatheringState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;

        private bool _readyToLeaveGatheringState;

        private float _stickDistance = 3f;
        
        public GatheringState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.PlayerGatheringEvent.OnPlayerGathering += PlayerGatheringEvent_OnPlayerGathering;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerGatheringEvent.OnPlayerGathering -= PlayerGatheringEvent_OnPlayerGathering;
        }

        public override void Tick()
        {
            _player.PlayerInteract.TryInteract();
            _player.PlayerLocomotion.HandleGroundedMovement();

            StickPlayerToResourcesGatherer();
        }

        private void StickPlayerToResourcesGatherer()
        {
            if (_player.LockedResourcesGatherer == null) return;
            
            if (Vector3.Distance(_player.transform.position, _player.LockedResourcesGatherer.position) <=
                _stickDistance)
            {
                _player.PlayerLocomotion.HandleStickRotation(_player.LockedResourcesGatherer);
                return;
            }

            if (_readyToLeaveGatheringState)
            {
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
            }
        }
        
        private void PlayerGatheringEvent_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            if (e.HasFullyGathered)
            {
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
                return;
            }
            
            _readyToLeaveGatheringState = !e.IsGathering;
        }
    }
}