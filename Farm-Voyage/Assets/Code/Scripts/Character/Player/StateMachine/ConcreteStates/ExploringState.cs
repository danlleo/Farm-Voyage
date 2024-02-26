using System;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class ExploringState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;

        private bool _startedGathering;
        
        public ExploringState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.PlayerFoundCollectableEvent.OnPlayerFoundCollectable += PlayerFoundCollectableEvent_OnPlayerFoundCollectable;
            _player.PlayerShoppingEvent.OnPlayerShopping += PlayerShoppingEvent_OnPlayerShopping;
            _player.PlayerUsingWorkbenchEvent.OnPlayerUsingWorkbench += PlayerUsingWorkbenchEvent_OnPlayerUsingWorkbench;
            _player.PlayerGatheringEvent.OnPlayerGathering += PlayerGatheringEvent_OnPlayerGathering;
            _player.PlayerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingStorageBoxStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerFoundCollectableEvent.OnPlayerFoundCollectable -= PlayerFoundCollectableEvent_OnPlayerFoundCollectable;
            _player.PlayerShoppingEvent.OnPlayerShopping -= PlayerShoppingEvent_OnPlayerShopping;
            _player.PlayerUsingWorkbenchEvent.OnPlayerUsingWorkbench -= PlayerUsingWorkbenchEvent_OnPlayerUsingWorkbench;
            _player.PlayerGatheringEvent.OnPlayerGathering -= PlayerGatheringEvent_OnPlayerGathering;
            _player.PlayerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingStorageBoxStateChanged;
        }

        public override void OnExit()
        {
            if (_startedGathering) return;
            
            _player.PlayerLocomotion.StopAllMovement();
        }
        
        public override void Tick()
        {
            _player.PlayerLocomotion.HandleGroundedMovement();
            _player.PlayerLocomotion.HandleRotation();
            _player.PlayerInteract.TryInteract();
        }
        
        private void PlayerFoundCollectableEvent_OnPlayerFoundCollectable(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.FoundCollectable());
        }
        
        private void PlayerShoppingEvent_OnPlayerShopping(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.Shopping());
        }
        
        private void PlayerUsingWorkbenchEvent_OnPlayerUsingWorkbench(object sender, PlayerUsingWorkbenchEventArgs e)
        {
            if (e.IsUsingWorkbench)
                _stateMachine.ChangeState(_player.StateFactory.UsingWorkbench());
        }
        
        private void PlayerGatheringEvent_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            if (!e.IsGathering) return;
            
            _startedGathering = true;
            _player.LockedResourcesGatherer = e.LockedTransform;
            _stateMachine.ChangeState(_player.StateFactory.Gathering());
        }
        
        // Свiдомий iвент
        private void PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingStorageBoxStateChanged(object sender, PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (e.IsCarrying)
                _stateMachine.ChangeState(_player.StateFactory.CarryingStorageBox());
        }
    }
}