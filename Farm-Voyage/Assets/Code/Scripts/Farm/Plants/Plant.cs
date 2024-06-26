using Attributes.WithinParent;
using Character.Player;
using Common;
using Farm.Corral;
using Misc;
using Sound;
using UnityEngine;

namespace Farm.Plants
{
    [RequireComponent(typeof(PlantFinishedWateringEvent))]
    [RequireComponent(typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public abstract class Plant : MonoBehaviour, IInteractable, IPlayerStopInteractable, IInteractDisplayProgress, ILockedInteract
    {
        public float MaxClampedProgress => 1f;
        public Observable<float> CurrentClampedProgress { get; } = new();
        
        public abstract PlantType Type { get; }
        
        public PlantFinishedWateringEvent PlantFinishedWateringEvent { get; private set; }
        
        public StateFactory StateFactory { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }
        public PlantArea PlantArea { get; private set; }

        [field: SerializeField, WithinParent, Header("External references")]
        public Transform PlantVisual { get; private set; }

        [field: SerializeField, Range(1f, 60f), Header("Settings")]
        public float PlantPartitionGrowTimeInSecond { get; private set; }

        [field: SerializeField, Range(0.1f, 2f)]
        public float PlantCompressedScale { get; private set; } = 0.75f;

        [field: SerializeField, Range(0.1f, 3f)]
        public float InitialScale { get; private set; }

        [field: SerializeField, Range(0.1f, 3f)]
        public float GrownScale { get; private set; }

        [field: SerializeField, Range(1, 5)] public int Partitions { get; private set; } = 2;
        
        [field: SerializeField] public AudioClip[] WateringAudioClips { get; private set; }

        [SerializeField] private AudioClip _selectAudioClip;
        [SerializeField] private AudioClip[] _harvestedPlantAudioClips;
        
        private StateMachine _stateMachine;

        protected virtual void Awake()
        {
            PlantFinishedWateringEvent = GetComponent<PlantFinishedWateringEvent>();
            _stateMachine = new StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
            PlantVisual.localScale = InitialScale * Vector3.one;
        }

        private void OnEnable()
        {
            _stateMachine.CurrentState?.SubscribeToEvents();
        }

        private void OnDisable()
        {
            _stateMachine.CurrentState.UnsubscribeFromEvents();
        }
        
        protected virtual void Start()
        {
            _stateMachine.Initialize(StateFactory.Growing());
        }

        private void OnDestroy()
        {
            _stateMachine.CurrentState.OnExit();
        }

        public void Initialize(Vector3 position, Quaternion rotation, PlantArea plantArea,
            PlayerInventory playerInventory)
        {
            transform.SetPositionAndRotation(position, rotation);
            PlantArea = plantArea;
            PlayerInventory = playerInventory;
        }

        public void Interact(IVisitable initiator)
        {
            _stateMachine.CurrentState.OnInteracted(initiator);
        }

        public void PlayerStopInteract(Player player)
        {
            _stateMachine.CurrentState.OnPlayerStoppedInteracting(player);
        }

        public void OnLockedInteract()
        {
            SoundFXManager.Instance.PlaySoundFX2DClip(_selectAudioClip,.4f);
        }
        
        public void OnHarvested()
        {
            SoundFXManager.Instance.PlayRandomSoundFX2DClip(_harvestedPlantAudioClips, .3f);
        }
    }
}
