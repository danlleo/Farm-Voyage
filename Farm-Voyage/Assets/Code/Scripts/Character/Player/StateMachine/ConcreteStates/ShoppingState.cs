namespace Character.Player.StateMachine.ConcreteStates
{
    public class ShoppingState : State
    {
        private Player _player;
        private StateMachine _stateMachine;
        
        public ShoppingState(Player player, StateMachine stateMachine) : base(player, stateMachine)
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

        private void PlayerShoppingEvent_OnPlayerShopping(object sender, PlayerShoppingEventArgs e)
        {
            if (e.IsShopping) return;

            _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}
