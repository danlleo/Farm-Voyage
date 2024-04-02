using Character.Player.Events;
using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class UsingWorkbenchState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public UsingWorkbenchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }
        public override void SubscribeToEvents()
        {
            _player.Events.ShoppingEvent.OnPlayerShopping += ShoppingEventOnShopping;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.Events.ShoppingEvent.OnPlayerShopping -= ShoppingEventOnShopping;
        }

        public override void OnEnter()
        {
            _player.Locomotion.HandleMoveDestination(_player.TransformPoints.WorkbenchStayPoint.position,
                Quaternion.identity);
        }

        private void ShoppingEventOnShopping(object sender, PlayerShoppingEventArgs e)
        {
            if (e.IsShopping) return;
        }
    }
}