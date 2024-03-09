using UnityEngine;

namespace Character.Player.StateMachine.ConcreteStates
{
    public class UsingWorkbenchState : State
    {
        private Player _player;
        private StateMachine _stateMachine;
        
        public UsingWorkbenchState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }
        public override void SubscribeToEvents()
        {
            _player.PlayerShoppingEvent.OnPlayerShopping += PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerShoppingEvent.OnPlayerShopping -= PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void OnEnter()
        {
            _player.PlayerLocomotion.HandleMoveDestination(_player.WorkbenchStayPoint.position, Quaternion.identity);
        }

        private void PlayerShoppingEvent_OnPlayerShopping(object sender, PlayerShoppingEventArgs e)
        {
            if (e.IsShopping) return;
        }
    }
}