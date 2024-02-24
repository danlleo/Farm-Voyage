using System.Collections.Generic;
using Attributes.WithinParent;
using Character.Player;
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
        [field:SerializeField, WithinParent] public Transform PlantVisual { get; private set; }
        
        public StateFactory StateFactory { get; private set; }
        public PlayerInventory PlayerInventory;
        
        public Vector3 CurrentScale => PlantVisual.localScale;
        public Vector3 TargetScale => _grownScale * Vector3.one;
        
        public float PlantPartitionGrowTimeInSecond => _plantPartitionGrowTimeInSeconds;
        public float PlantCompressedScale => _plantCompressedScale;
        public List<float> WateringThresholds => _wateringThresholds;
        
        [Header("Settings")]
        [SerializeField, Range(0.1f, 3f)] private float _grownScale;
        [SerializeField, Range(0.1f, 3f)] private float _initialScale;
        [SerializeField, Range(0.1f, 2f)] private float _plantCompressedScale = 0.75f;
        [SerializeField, Range(1f, 60f)] private float _plantPartitionGrowTimeInSeconds;
        [SerializeField, Range(0.1f, 0.9f)] private List<float> _wateringThresholds = new() { 0.25f, 0.65f, 0.9f };
        
        private StateMachine _stateMachine;
        private PlantArea _plantArea;

        private BoxCollider _boxCollider;
        
        protected virtual void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _stateMachine = new StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
            PlantVisual.localScale = _initialScale * Vector3.one;
        }

        protected virtual void Start()
        {
            _stateMachine.Initialize(StateFactory.Growing());
        }

        public void Initialize(Vector3 position, Quaternion rotation, PlantArea plantArea, PlayerInventory playerInventory)
        {
            transform.SetPositionAndRotation(position, rotation);
            _plantArea = plantArea;
            PlayerInventory = playerInventory;
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
