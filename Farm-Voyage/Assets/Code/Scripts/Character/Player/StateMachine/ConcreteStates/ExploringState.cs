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
            _player.PlayerEvents.PlayerFoundCollectableEvent.OnPlayerFoundCollectable += Player_OnPlayerFoundCollectable;
            _player.PlayerEvents.PlayerShoppingEvent.OnPlayerShopping += Player_OnPlayerShopping;
            _player.PlayerEvents.PlayerUsingWorkbenchEvent.OnPlayerUsingWorkbench += Player_OnPlayerUsingWorkbench;
            _player.PlayerEvents.PlayerGatheringEvent.OnPlayerGathering += Player_OnPlayerGathering;
            _player.PlayerEvents.PlayerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                Player_OnPlayerCarryingStorageBoxStateChanged;
            _player.PlayerEvents.PlayerEnteringHomeEvent.OnPlayerEnteringHome += Player_OnPlayerEnteringHome;
            _player.PlayerEvents.PlayerExtractingWaterEvent.OnPlayerExtractingWater += Player_OnPlayerExtractingWater;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerEvents.PlayerFoundCollectableEvent.OnPlayerFoundCollectable -= Player_OnPlayerFoundCollectable;
            _player.PlayerEvents.PlayerShoppingEvent.OnPlayerShopping -= Player_OnPlayerShopping;
            _player.PlayerEvents.PlayerUsingWorkbenchEvent.OnPlayerUsingWorkbench -= Player_OnPlayerUsingWorkbench;
            _player.PlayerEvents.PlayerGatheringEvent.OnPlayerGathering -= Player_OnPlayerGathering;
            _player.PlayerEvents.PlayerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                Player_OnPlayerCarryingStorageBoxStateChanged;
            _player.PlayerEvents.PlayerEnteringHomeEvent.OnPlayerEnteringHome -= Player_OnPlayerEnteringHome;
            _player.PlayerEvents.PlayerExtractingWaterEvent.OnPlayerExtractingWater -= Player_OnPlayerExtractingWater;
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
        
        private void Player_OnPlayerFoundCollectable(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.FoundCollectable());
        }
        
        private void Player_OnPlayerShopping(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.Shopping());
        }
        
        private void Player_OnPlayerUsingWorkbench(object sender, PlayerUsingWorkbenchEventArgs e)
        {
            if (!e.IsUsingWorkbench) return;
            
            _stateMachine.ChangeState(_player.StateFactory.UsingWorkbench());
        }
        
        private void Player_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            if (!e.IsGathering) return;
            
            _startedGathering = true;
            _stateMachine.ChangeState(_player.StateFactory.Gathering(e.LockedTransform));
        }
        
        private void Player_OnPlayerCarryingStorageBoxStateChanged(object sender, PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (!e.IsCarrying) return;
            
            _stateMachine.ChangeState(_player.StateFactory.CarryingStorageBox());
        }
        
        private void Player_OnPlayerEnteringHome(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.EnteringHome());
        }
        
        private void Player_OnPlayerExtractingWater(object sender, PlayerExtractingWaterEventArgs e)
        {
            if (!e.IsExtracting) return;
            
            _stateMachine.ChangeState(_player.StateFactory.ExtractingWater());
        }
    }
}