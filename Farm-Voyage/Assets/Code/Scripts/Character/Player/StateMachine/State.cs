namespace Character.Player.StateMachine
{
    public class State
    {
        protected readonly Player Player;
        protected readonly StateMachine StateMachine;

        protected State(Player player, StateMachine stateMachine)
        {
            Player = player;
            StateMachine = stateMachine;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Tick() { }
    }
}