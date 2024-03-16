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
    public abstract class Plant : MonoBehaviour, IInteractable, IStopInteractable
    {
        public StateFactory StateFactory { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }
        public PlantArea PlantArea { get; private set; }
        
        [field:SerializeField, WithinParent, Header("External references")] public Transform PlantVisual { get; private set; }
        
        [field: SerializeField, Range(1f, 60f), Header("Settings")] public float PlantPartitionGrowTimeInSecond { get; private set; }
        
        [field: SerializeField, Range(0.1f, 2f)] public float PlantCompressedScale { get; private set; } = 0.75f;
        [field: SerializeField, Range(0.1f, 3f)] public float InitialScale { get; private set; }
        [field: SerializeField, Range(0.1f, 3f)] public float GrownScale { get; private set; }
        [field: SerializeField, Range(1, 5)] public int Partitions { get; private set; } = 2;
        
        private StateMachine _stateMachine;

        private BoxCollider _boxCollider;
        
        protected virtual void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _stateMachine = new StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
            PlantVisual.localScale = InitialScale * Vector3.one;
        }

        protected virtual void Start()
        {
            _stateMachine.Initialize(StateFactory.Growing());
        }

        public void Initialize(Vector3 position, Quaternion rotation, PlantArea plantArea, PlayerInventory playerInventory)
        {
            transform.SetPositionAndRotation(position, rotation);
            PlantArea = plantArea;
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
            PlantArea.ClearPlantArea();
        }
    }
}
