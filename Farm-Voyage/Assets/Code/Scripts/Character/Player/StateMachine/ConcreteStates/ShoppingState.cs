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
    }
}
