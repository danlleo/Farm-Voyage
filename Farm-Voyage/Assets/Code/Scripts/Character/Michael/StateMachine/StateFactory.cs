using Character.Michael.StateMachine.ConcreteStates;

namespace Character.Michael.StateMachine
{
    public class StateFactory
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;

        public StateFactory(Michael michael, StateMachine stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public State Gardening()
        {
            return new GardeningState(_michael, _stateMachine);
        }
    }
}