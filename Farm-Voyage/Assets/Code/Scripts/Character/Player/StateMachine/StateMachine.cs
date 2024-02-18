namespace Character.Player.StateMachine
{
    public sealed class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State initialState)
        {
            CurrentState = initialState;
            CurrentState.OnEnter();
            CurrentState.SubscribeToEvents();
        }

        public void ChangeState(State targetState)
        {
            CurrentState.OnExit();
            CurrentState = targetState;
            CurrentState.OnEnter();
        }
    }
}
