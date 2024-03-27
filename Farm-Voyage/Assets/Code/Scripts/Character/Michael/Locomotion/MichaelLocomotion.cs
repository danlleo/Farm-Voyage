using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

namespace Character.Michael.Locomotion
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(MichaelLocomotionStateChangedEvent))]
    [DisallowMultipleComponent]
    public class MichaelLocomotion : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Range(0.1f, 20f)] private float _movingSpeed;
        [SerializeField, Range(80f, 300f)] private float _rotationSpeed;
        [SerializeField, Range(0f, 20f)] private float _stoppingDistance = 1f;
        
        private NavMeshAgent _navMeshAgent;
        private MichaelLocomotionStateChangedEvent _michaelLocomotionStateChangedEvent;
        
        private Coroutine _moveDestinationRoutine;
        
        private void Awake()
        {
            _michaelLocomotionStateChangedEvent = GetComponent<MichaelLocomotionStateChangedEvent>();
            InitializeNavMeshAgent();
        }

        public void HandleMoveDestination(Transform destination, Action onReachedDestination)
        {
            HandleMoveDestination(destination.position, onReachedDestination);
        }

        public void HandleMoveDestination(Vector3 destination, Action onReachedDestination)
        {
            CoroutineHandler.StopCoroutine(this, _moveDestinationRoutine);
            CoroutineHandler.StartAndOverride(this, ref _moveDestinationRoutine,
                MoveDestinationRoutine(destination, onReachedDestination));
        }

        public void StopAllMovement()
        {
            CoroutineHandler.StopCoroutine(this, _moveDestinationRoutine);
        }

        private void InitializeNavMeshAgent()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _movingSpeed;
            _navMeshAgent.angularSpeed = _rotationSpeed;
        }
        
        private IEnumerator MoveDestinationRoutine(Vector3 destination, Action onReachedPosition)
        {
            destination = new Vector3(destination.x, 0f, destination.z);

            _navMeshAgent.isStopped = false;
            _michaelLocomotionStateChangedEvent.Call(true);
            _navMeshAgent.SetDestination(destination);

            bool isMoving = true;

            while (isMoving)
            {
                if (Vector3.Distance(transform.position, destination) <= _stoppingDistance)
                {
                    _navMeshAgent.isStopped = true;
                    isMoving = false;
                }

                yield return null;
            }
    
            _michaelLocomotionStateChangedEvent.Call(false);
            onReachedPosition?.Invoke();
        }
    }
}
