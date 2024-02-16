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

        public override void OnEnter()
        {
            _player.PlayerFoundCollectableEvent.OnPlayerFoundCollectable += PlayerFoundCollectableEvent_OnPlayerFoundCollectable;
        }

        public override void OnExit()
        {
            _player.PlayerFoundCollectableEvent.OnPlayerFoundCollectable -= PlayerFoundCollectableEvent_OnPlayerFoundCollectable;
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
    }
}