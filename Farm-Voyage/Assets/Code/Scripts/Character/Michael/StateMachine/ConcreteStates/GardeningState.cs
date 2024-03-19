using System.ComponentModel;
using UnityEngine;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class GardeningState : State
    {
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;
        
        public GardeningState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _michael.Waterar.PlantToNeedsWateringMapping.PropertyChanged += HandlePropertyChange;            
        }

        public override void UnsubscribeFromEvents()
        {
            _michael.Waterar.PlantToNeedsWateringMapping.PropertyChanged -= HandlePropertyChange;            
        }

        private void HandlePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            Debug.Log(e.PropertyName);
        }
    }
}