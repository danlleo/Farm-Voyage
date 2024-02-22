namespace Character.Player.StateMachine
{
    public sealed class StateMachine
    {
        public State CurrentState { get; private set; }

        public void Initialize(State initialState)
        {
            CurrentState = initialState;
            CurrentState.SubscribeToEvents();
            CurrentState.OnEnter();
        }

        public void ChangeState(State targetState)
        {
            CurrentState.UnsubscribeFromEvents();
            CurrentState.OnExit();
            CurrentState = targetState;
            CurrentState.SubscribeToEvents();
            CurrentState.OnEnter();
        }
    }
}
