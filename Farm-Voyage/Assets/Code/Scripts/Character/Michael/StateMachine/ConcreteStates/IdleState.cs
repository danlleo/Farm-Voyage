using System.Collections;
using Farm.Plants;
using UnityEngine;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class IdleState : State
    {
        private const float TimeToStayInIdleInSeconds = 10f;
        
        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;

        private Coroutine _stayInIdleStateRoutine;
        
        public IdleState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public override void SubscribeToEvents()
        {
            _michael.WaterStateObserver.OnPlantWateringStateChanged += WaterStateObserver_OnPlantWateringStateChanged;
        }

        public override void UnsubscribeFromEvents()
        {
            _michael.WaterStateObserver.OnPlantWateringStateChanged -= WaterStateObserver_OnPlantWateringStateChanged;
        }

        public override void OnEnter()
        {
            _michael.Events.MichaelSittingStateChangedEvent.Call(true);
            _stayInIdleStateRoutine = _michael.StartCoroutine(StayInIdleStateRoutine());
        }

        public override void OnExit()
        {
            _michael.Events.MichaelSittingStateChangedEvent.Call(false);
        }

        private void ProceedToWalkIfPlantNeedsWatering(Plant plant, bool needsWatering)
        {
            if (!needsWatering) return;
            
            _michael.StopCoroutine(_stayInIdleStateRoutine);
            _stateMachine.ChangeState(_michael.StateFactory.WalkingToPlant(plant));
        }

        private IEnumerator StayInIdleStateRoutine()
        {
            yield return new WaitForSeconds(TimeToStayInIdleInSeconds);
            _stateMachine.ChangeState(_michael.StateFactory.Gardening());
        }
        
        private void WaterStateObserver_OnPlantWateringStateChanged(Plant plant, bool needsWatering)
        {
            ProceedToWalkIfPlantNeedsWatering(plant, needsWatering);
        }
    }
}