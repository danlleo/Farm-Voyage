namespace Character.Michael.StateMachine
{
    public class State
    {
        protected readonly Michael Michael;
        protected readonly StateMachine StateMachine;

        public State(Michael michael, StateMachine stateMachine)
        {
            Michael = michael;
            StateMachine = stateMachine;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void SubscribeToEvents() { }
        public virtual void UnsubscribeFromEvents() { }
        public virtual void Tick() { }
    }
}