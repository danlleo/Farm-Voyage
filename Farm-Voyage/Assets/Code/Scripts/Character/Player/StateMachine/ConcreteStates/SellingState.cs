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
    }
}