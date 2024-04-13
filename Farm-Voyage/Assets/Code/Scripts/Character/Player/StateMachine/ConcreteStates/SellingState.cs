using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class SellingState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public SellingState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.Events.SellingStateChangedEvent.OnStartedSellingStateChanged += Player_OnStartedSellingStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.SellingStateChangedEvent.OnStartedSellingStateChanged -= Player_OnStartedSellingStateChanged;
        }

        public override void OnEnter()
        {
            _player.Locomotion.HandleMoveDestination(_player.TransformPoints.SellerStayPoint, Quaternion.identity);
        }
        
        private void Player_OnStartedSellingStateChanged(bool isSelling)
        {
            if (isSelling) return;
            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}