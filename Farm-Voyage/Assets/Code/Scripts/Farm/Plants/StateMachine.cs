namespace Farm.Plants
{
    public sealed class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State initialState)
        {
            CurrentState = initialState;
            CurrentState.OnEnter();
        }

        public void ChangeState(State targetState)
        {
            CurrentState.OnExit();
            CurrentState = targetState;
            CurrentState.OnEnter();
        }
    }
}