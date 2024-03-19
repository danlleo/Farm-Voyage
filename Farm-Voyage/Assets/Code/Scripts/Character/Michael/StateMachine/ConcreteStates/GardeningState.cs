namespace Character.Michael.StateMachine.ConcreteStates
{
    public class GardeningState : State
    {
        private Michael _michael;
        private StateMachine _stateMachine;
        
        public GardeningState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }
    }
}