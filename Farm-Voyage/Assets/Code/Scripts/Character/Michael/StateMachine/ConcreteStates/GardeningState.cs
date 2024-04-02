using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Character.Michael.StateMachine.ConcreteStates
{
    public class GardeningState : State
    {
        private const int MinimalRouteLengthInclusive = 2;
        private const int MaximalRouteLengthExclusive = 7;

        private readonly Michael _michael;
        private readonly StateMachine _stateMachine;

        private Vector3[] _routePositions;

        public GardeningState(Michael michael, StateMachine stateMachine) : base(michael, stateMachine)
        {
            _michael = michael;
            _stateMachine = stateMachine;
        }

        public override void OnEnter()
        {
            GenerateRoute();
            StartGardening();
        }

        private void GenerateRoute()
        {
            int randomRouteLength = Random.Range(MinimalRouteLengthInclusive, MaximalRouteLengthExclusive);
            _routePositions = new Vector3[randomRouteLength];

            Transform firstRandomPoint = _michael.TransformPoints.GardeningPoints.GetRandomItem();
            _routePositions[0] = firstRandomPoint.position;

            List<Transform> excludedTransforms = new() { firstRandomPoint };

            for (int i = 1; i < randomRouteLength; i++)
            {
                Transform nextPoint =
                    _michael.TransformPoints.GardeningPoints.GetRandomItemExcept(excludedTransforms.ToArray());
                _routePositions[i] = nextPoint.position;
                excludedTransforms.Add(nextPoint);
            }
        }

        private void StartGardening()
        {
            _michael.StartCoroutine(GardenRoutine());
        }
        
        private IEnumerator GardenRoutine()
        {
            foreach (Vector3 routePosition in _routePositions)
            {
                bool hasArrived = false;
                bool hasFinishedAction = false;
                
                _michael.Locomotion.HandleMoveDestination(routePosition, () => hasArrived = true);
        
                yield return new WaitUntil(() => hasArrived);

                _michael.Events.PerformingGardeningActionEvent.Call(
                    EnumUtility.GetRandomEnumValue<GardeningActionType>(),
                    () => hasFinishedAction = true);
            
                yield return new WaitUntil(() => hasFinishedAction);
            }

            _stateMachine.ChangeState(_michael.StateFactory.WalkingToIdlePosition());
        }
    }
}