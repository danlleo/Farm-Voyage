using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class ShoppingState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public ShoppingState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.PlayerEvents.PlayerShoppingEvent.OnPlayerShopping += PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerEvents.PlayerShoppingEvent.OnPlayerShopping -= PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void OnEnter()
        {
            _player.PlayerLocomotion.HandleMoveDestination(_player.TransformPoints.EmmaStoreStayPoint.position,
                Quaternion.identity);
        }

        private void PlayerShoppingEvent_OnPlayerShopping(object sender, PlayerShoppingEventArgs e)
        {
            if (e.IsShopping) return;

            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}
