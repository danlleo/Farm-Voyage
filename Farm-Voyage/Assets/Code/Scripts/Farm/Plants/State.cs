using Character;
using Character.Player;
using Common;

namespace Farm.Plants
{
    public class State
    {
        protected readonly Plant Plant;
        protected readonly StateMachine StateMachine;

        protected State(Plant plant, StateMachine stateMachine)
        {
            Plant = plant;
            StateMachine = stateMachine;
        }
        
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void SubscribeToEvents() { }
        public virtual void UnsubscribeFromEvents() { }
        public virtual void Tick() { }
        public virtual void OnInteracted(IVisitable initiator) { }
        public virtual void OnPlayerStoppedInteracting(Player player) { }
    }
}