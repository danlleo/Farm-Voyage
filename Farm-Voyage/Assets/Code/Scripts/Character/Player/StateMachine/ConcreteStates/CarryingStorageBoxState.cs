namespace Character.Player.StateMachine.ConcreteStates
{
    public class CarryingStorageBoxState : State
    {
        private readonly Player _player;
        private readonly StateMachine _stateMachine;
        
        public CarryingStorageBoxState(Player player, StateMachine stateMachine) : base(player, stateMachine)
        {
            _player = player;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _player.PlayerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                Player_OnPlayerCarryingStorageBoxStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _player.PlayerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                Player_OnPlayerCarryingStorageBoxStateChanged;
        }

        private void Player_OnPlayerCarryingStorageBoxStateChanged(object sender, PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            if (!e.IsCarrying)
                _stateMachine.ChangeState(_player.StateFactory.Exploring());
        }
    }
}