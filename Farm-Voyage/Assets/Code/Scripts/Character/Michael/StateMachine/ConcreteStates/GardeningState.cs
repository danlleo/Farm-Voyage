using System;
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
            
            Debug.Log(_routePositions.Length);
        }

        private void StartGardening()
        {
            _michael.StartCoroutine(GardenRoutine());
        }
        
        private IEnumerator GardenRoutine()
        {
            foreach (Vector3 routePosition in _routePositions)
            {
                // Assuming you've set up a way to wait for the movement to complete, such as an event or a condition.
                bool hasArrived = false;
        
                // Call the wrapped or modified HandleMoveDestination method with a callback
                _michael.MichaelLocomotion.HandleMoveDestination(routePosition, () => hasArrived = true);
        
                // Wait until the callback sets hasArrived to true
                yield return new WaitUntil(() => hasArrived);
            }

            // All points have been visited, transition to the next state.
            _stateMachine.ChangeState(_michael.StateFactory.Idle());
        }
    }
}