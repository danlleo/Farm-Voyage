using Common;
using UnityEngine;

namespace Farm.Plants
{
    [DisallowMultipleComponent]
    public class Plant : MonoBehaviour, IInteractable
    {
        public StateFactory StateFactory { get; private set; }
        public Vector3 InitialScale => _initialScale * Vector3.one;
        public Vector3 CurrentScale => transform.localScale;
        public Vector3 TargetScale => _grownScale * Vector3.one;
        public float PlantPartitionGrowTimeInSecond => _plantPartitionGrowTimeInSeconds;
        public float[] WateringThresholds => _wateringThresholds;
        
        [Header("Settings")]
        [SerializeField, Range(0.1f, 3f)] public float _grownScale;
        [SerializeField, Range(0.1f, 3f)] private float _initialScale; 
        [SerializeField, Range(1f, 60f)] private float _plantPartitionGrowTimeInSeconds;
        [SerializeField, Range(0.1f, 1f)] private float[] _wateringThresholds = { 0.25f, 0.65f, 1f };
        
        private StateMachine _stateMachine;
        
        private void Awake()
        {
            _stateMachine = new StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
            transform.localScale = _initialScale * Vector3.one;
        }

        private void Start()
        {
            _stateMachine.Initialize(StateFactory.Growing());
        }

        private void Update()
        {
            _stateMachine.CurrentState.Tick();
        }

        public void Interact()
        {
            _stateMachine.CurrentState.OnInteracted();
        }

        public void StopInteract()
        {
            _stateMachine.CurrentState.OnStoppedInteracting();
        }
    }
}
