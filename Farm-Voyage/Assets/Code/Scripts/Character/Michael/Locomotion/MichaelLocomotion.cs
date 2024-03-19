using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Character.Michael.Locomotion
{
    [SelectionBase]
    [RequireComponent(typeof(NavMeshAgent))]
    [DisallowMultipleComponent]
    public class MichaelLocomotion : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Range(0.1f, 20f)] private float _movingSpeed;
        [SerializeField, Range(80f, 200f)] private float _rotationSpeed;
        
        private NavMeshAgent _navMeshAgent;

        private Coroutine _moveDestinationRoutine;
        
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _movingSpeed;
            _navMeshAgent.angularSpeed = _rotationSpeed;
        }

        public void HandleMoveDestination(Vector3 destination, Action onReachedDestination)
        {
            if (_moveDestinationRoutine != null)
                StopCoroutine(_moveDestinationRoutine);

            _moveDestinationRoutine = StartCoroutine(MoveDestinationRoutine(destination, onReachedDestination));
        }

        public void StopAllMovement()
        {
            StopCoroutine(_moveDestinationRoutine);
        }

        private IEnumerator MoveDestinationRoutine(Vector3 destination, Action onReachedPosition)
        {
            _navMeshAgent.destination = destination;

            while (_navMeshAgent.remainingDistance > 0f)
            {
                yield return null;
            }
            
            onReachedPosition?.Invoke();
        }
    }
}