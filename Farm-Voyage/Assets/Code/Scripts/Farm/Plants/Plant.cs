using Common;
using Farm.Corral;
using UnityEngine;
using Utilities;

namespace Farm.Plants
{
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public abstract class Plant : MonoBehaviour, IInteractable
    {
        public StateFactory StateFactory { get; private set; }
        public Vector3 CurrentScale => transform.localScale;
        public Vector3 TargetScale => _grownScale * Vector3.one;
        public float PlantPartitionGrowTimeInSecond => _plantPartitionGrowTimeInSeconds;
        public float[] WateringThresholds => _wateringThresholds;
        
        [Header("Settings")]
        [SerializeField, Range(0.1f, 3f)] private float _grownScale;
        [SerializeField, Range(0.1f, 3f)] private float _initialScale; 
        [SerializeField, Range(1f, 60f)] private float _plantPartitionGrowTimeInSeconds;
        [SerializeField, Range(0.1f, 1f)] private float[] _wateringThresholds = { 0.25f, 0.65f, 1f };
        
        private StateMachine _stateMachine;
        private PlantArea _plantArea;

        private BoxCollider _boxCollider;
        
        protected virtual void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _stateMachine = new StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
            transform.localScale = _initialScale * Vector3.one;
        }

        protected virtual void Start()
        {
            _stateMachine.Initialize(StateFactory.Growing());
        }

        public void Initialize(Vector3 position, Quaternion rotation, PlantArea plantArea)
        {
            transform.SetPositionAndRotation(position, rotation);
            _plantArea = plantArea;
        }
        
        public void Interact()
        {
            _stateMachine.CurrentState.OnInteracted();
        }

        public void StopInteract()
        {
            _stateMachine.CurrentState.OnStoppedInteracting();
        }

        public void Harvest()
        {
            _boxCollider.Disable();
            _plantArea.ClearPlantArea();
        }
    }
}
