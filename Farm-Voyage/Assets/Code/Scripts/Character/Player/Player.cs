using Attributes.Self;
using Character.Player.StateMachine;
using Farm.Corral;
using InputManagers;
using Misc;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerInteract))]
    [RequireComponent(typeof(PlayerGatheringEvent))]
    [RequireComponent(typeof(PlayerCarryingStorageBoxStateChangedEvent))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IValidate
    {
        public const float Height = 1f;
        public const float Radius = .5f;

        public bool IsValid { get; private set; } = true;
        
        public StateFactory StateFactory { get; private set; }
        
        public PlayerInteract PlayerInteract { get; private set; }
        public PlayerLocomotion PlayerLocomotion { get; private set; }
        
        public PlayerWalkingEvent PlayerWalkingEvent { get; private set; }
        public PlayerIdleEvent PlayerIdleEvent { get; private set; }
        public PlayerGatheringEvent PlayerGatheringEvent { get; private set; }
        public PlayerDiggingPlantAreaEvent PlayerDiggingPlantAreaEvent { get; private set; }
        public PlayerCarryingStorageBoxStateChangedEvent PlayerCarryingStorageBoxStateChangedEvent { get; private set; }
        
        public PlayerInput Input { get; private set; }
        
        [Header("External references")]
        [SerializeField, Self] private Transform _carryPoint;

        private StateMachine.StateMachine _stateMachine;
        
        private PlayerMapLimitBoundaries _playerMapLimitBoundaries;
        private StorageBox _storageBox;
        
        [Inject]
        private void Construct(PlayerInput playerInput)
        {
            Input = playerInput;
        }
        
        private void Awake()
        {
            PlayerWalkingEvent = GetComponent<PlayerWalkingEvent>();
            PlayerIdleEvent = GetComponent<PlayerIdleEvent>();
            PlayerGatheringEvent = GetComponent<PlayerGatheringEvent>();
            PlayerDiggingPlantAreaEvent = GetComponent<PlayerDiggingPlantAreaEvent>();
            PlayerCarryingStorageBoxStateChangedEvent = GetComponent<PlayerCarryingStorageBoxStateChangedEvent>();
            PlayerLocomotion = GetComponent<PlayerLocomotion>();
            PlayerInteract = GetComponent<PlayerInteract>();
            
            _playerMapLimitBoundaries = new PlayerMapLimitBoundaries(transform, -25, 25, -25, 25);
            _stateMachine = new StateMachine.StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
        }

        private void Start()
        {
            _stateMachine.Initialize(StateFactory.Exploring());
        }

        private void Update()
        {
            _stateMachine.CurrentState.Tick();
            _playerMapLimitBoundaries.KeepWithinBoundaries();
        }

        private void OnValidate()
        {
            IsValid = true;

            if (_carryPoint == null)
            {
                IsValid = false;
            }
        }

        public void CarryStorageBox(StorageBox storageBox)
        {
            if (_storageBox != null) return;

            PlayerCarryingStorageBoxStateChangedEvent.Call(this,
                new PlayerCarryingStorageBoxStateChangedEventArgs(true));
            
            storageBox.transform.SetParent(_carryPoint);
            storageBox.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);

            _storageBox = storageBox;
        }

        public bool TryGetStorageBox(out StorageBox storageBox)
        {
            if (_storageBox == null)
            {
                storageBox = null;
                return false;
            }

            storageBox = _storageBox;
            return true;
        }
    }
}
