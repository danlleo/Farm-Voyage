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

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _movingSpeed;
            _navMeshAgent.angularSpeed = _rotationSpeed;
        }

        public void HandleDestination(Vector3 destination)
        {
            _navMeshAgent.destination = destination;
        }
    }
}