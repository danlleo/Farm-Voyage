namespace Character.Michael.StateMachine.ConcreteStates
{
    public class GardeningState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        
        public GardeningState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }
        
        
    }
}