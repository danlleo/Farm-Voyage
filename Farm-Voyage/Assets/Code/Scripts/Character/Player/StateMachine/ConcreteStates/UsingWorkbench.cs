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
            _player.PlayerEvents.PlayerShoppingEvent.OnPlayerShopping += PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerEvents.PlayerShoppingEvent.OnPlayerShopping -= PlayerShoppingEvent_OnPlayerShopping;
        }

        public override void OnEnter()
        {
            _player.PlayerLocomotion.HandleMoveDestination(_player.TransformPoints.WorkbenchStayPoint.position,
                Quaternion.identity);
        }

        private void PlayerShoppingEvent_OnPlayerShopping(object sender, PlayerShoppingEventArgs e)
        {
            if (e.IsShopping) return;
        }
    }
}