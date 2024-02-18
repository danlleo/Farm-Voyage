using System;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class ExploringState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public ExploringState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.PlayerFoundCollectableEvent.OnPlayerFoundCollectable += PlayerFoundCollectableEvent_OnPlayerFoundCollectable;
            _player.PlayerShoppingEvent.OnPlayerShopping += PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerFoundCollectableEvent.OnPlayerFoundCollectable -= PlayerFoundCollectableEvent_OnPlayerFoundCollectable;
            _player.PlayerShoppingEvent.OnPlayerShopping -= PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void OnExit()
        {
            _player.PlayerLocomotion.StopAllMovement();
        }
        
        public override void Tick()
        {
            _player.PlayerLocomotion.HandleAllMovement();
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
    }
}