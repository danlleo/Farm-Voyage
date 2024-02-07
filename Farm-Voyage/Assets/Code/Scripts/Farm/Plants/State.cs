namespace Farm.Plants
{
    public class State
    {
        protected readonly Plant Plant;
        protected readonly StateMachine StateMachine;

        public State(Plant plant, StateMachine stateMachine)
        {
            Plant = plant;
            StateMachine = stateMachine;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Tick() { }
    }
}