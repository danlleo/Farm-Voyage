using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        [SerializeField, Range(80f, 200f)] private float _rotationSpeed;
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
            if (_moveDestinationRoutine != null)
            {
                StopCoroutine(_moveDestinationRoutine);
            }

            _moveDestinationRoutine = StartCoroutine(MoveDestinationRoutine(destination, onReachedDestination));
        }

        public void StopAllMovement()
        {
            StopCoroutine(_moveDestinationRoutine);
        }

        private void InitializeNavMeshAgent()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _movingSpeed;
            _navMeshAgent.angularSpeed = _rotationSpeed;
            _navMeshAgent.stoppingDistance = _stoppingDistance;
        }
        
        private IEnumerator MoveDestinationRoutine(Vector3 destination, Action onReachedPosition)
        {
            destination = new Vector3(destination.x, 0f, destination.z);
            
            _michaelLocomotionStateChangedEvent.Call(true);
            _navMeshAgent.SetDestination(destination);

            bool isMoving = true;

            while (isMoving)
            {
                if (_navMeshAgent.pathPending)
                    yield return null;
            
                while (_navMeshAgent.remainingDistance > 0f)
                {
                    yield return null;
                }

                isMoving = false;
            }
    
            _michaelLocomotionStateChangedEvent.Call(false);
            onReachedPosition?.Invoke();
        }
    }
}
