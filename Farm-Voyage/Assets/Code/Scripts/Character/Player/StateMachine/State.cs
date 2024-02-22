namespace Character.Player.StateMachine
{
    public class State
    {
        protected readonly Player Player;
        protected readonly StateMachine StateMachine;

        public State(Player player, StateMachine stateMachine)
        {
            Player = player;
            StateMachine = stateMachine;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void SubscribeToEvents() { }
        public virtual void UnsubscribeFromEvents() { }
        public virtual void Tick() { }
    }
}