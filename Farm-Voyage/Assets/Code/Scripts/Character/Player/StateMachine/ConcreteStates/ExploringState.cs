using System;
using Character.Player.Events;

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
            _player.Events.FoundCollectableEvent.OnPlayerFoundCollectable += Player_OnFoundCollectable;
            _player.Events.ShoppingEvent.OnPlayerShopping += Player_OnShopping;
            _player.Events.UsingWorkbenchStateChangedEvent.OnPlayerUsingWorkbenchStateChanged +=
                Player_OnUsingWorkbenchStateChanged;
            _player.Events.GatheringEvent.OnPlayerGathering += Player_OnGathering;
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                Player_OnCarryingStorageBoxStateChanged;
            _player.Events.EnteringHomeEvent.OnPlayerEnteringHome += Player_OnEnteringHome;
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged +=
                Player_OnExtractingWaterStateChanged;
            _player.Events.SellingStateChangedEvent.OnStartedSellingStateChanged +=
                Player_OnSellingStateChangedStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.FoundCollectableEvent.OnPlayerFoundCollectable -= Player_OnFoundCollectable;
            _player.Events.ShoppingEvent.OnPlayerShopping -= Player_OnShopping;
            _player.Events.UsingWorkbenchStateChangedEvent.OnPlayerUsingWorkbenchStateChanged -=
                Player_OnUsingWorkbenchStateChanged;
            _player.Events.GatheringEvent.OnPlayerGathering -= Player_OnGathering;
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                Player_OnCarryingStorageBoxStateChanged;
            _player.Events.EnteringHomeEvent.OnPlayerEnteringHome -= Player_OnEnteringHome;
            _player.Events.ExtractingWaterStateChangedEvent.OnPlayerExtractingWaterStateChanged -=
                Player_OnExtractingWaterStateChanged;
            _player.Events.SellingStateChangedEvent.OnStartedSellingStateChanged -=
                Player_OnSellingStateChangedStateChanged;
        }

        public override void OnExit()
        {
            if (_startedGathering) return;
            
            _player.Locomotion.StopAllMovement();
        }
        
        public override void Tick()
        {
            _player.Locomotion.HandleGroundedMovement();
            _player.Locomotion.HandleRotation();
            _player.Interact.TryInteract();
        }
        
        private void Player_OnFoundCollectable(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.FoundCollectable());
        }
        
        private void Player_OnShopping(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.Shopping());
        }

        private void Player_OnUsingWorkbenchStateChanged(bool isUsingWorkbench)
        {
            if (isUsingWorkbench) return;

            _stateMachine.ChangeState(_player.StateFactory.UsingWorkbench());
        }

        private void Player_OnGathering(object sender, PlayerGatheringEventArgs e)
        {
            if (!e.IsGathering) return;
            
            _startedGathering = true;
            _stateMachine.ChangeState(_player.StateFactory.Gathering(e.LockedTransform));
        }

        private void Player_OnCarryingStorageBoxStateChanged(object sender,
            PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (!e.IsCarrying) return;

            _stateMachine.ChangeState(_player.StateFactory.CarryingStorageBox());
        }

        private void Player_OnEnteringHome(object sender, EventArgs e)
        {
            _stateMachine.ChangeState(_player.StateFactory.EnteringHome());
        }
        
        private void Player_OnExtractingWaterStateChanged(bool isExtractingWater)
        {
            if (isExtractingWater) return;
            
            _stateMachine.ChangeState(_player.StateFactory.ExtractingWater());
        }
        
        private void Player_OnSellingStateChangedStateChanged(bool isSelling)
        {
            if (!isSelling) return;
            _stateMachine.ChangeState(_player.StateFactory.Selling());
        }
    }
}