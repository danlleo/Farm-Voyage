using Attributes.Self;
using Farm.Corral;
using InputManagers;
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
    [RequireComponent(typeof(PlayerInventory))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour
    {
        public const float Height = 1f;
        public const float Radius = .5f;
     
        public PlayerWalkingEvent PlayerWalkingEvent { get; private set; }
        public PlayerIdleEvent PlayerIdleEvent { get; private set; }
        public PlayerGatheringEvent PlayerGatheringEvent { get; private set; }
        public PlayerDiggingPlantAreaEvent PlayerDiggingPlantAreaEvent { get; private set; }
        public PlayerCarryingStorageBoxStateChangedEvent PlayerCarryingStorageBoxStateChangedEvent { get; private set; }
        public PlayerInput Input { get; private set; }
        public PlayerInventory Inventory { get; private set; }
        
        [Header("External references")]
        [SerializeField, Self] private Transform _carryPoint;

        private PlayerMapLimitBoundaries _playerMapLimitBoundaries;
        private StorageBox _storageBox;
        
        private PlayerInteract _playerInteract;
        private PlayerLocomotion _playerLocomotion;

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
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerInteract = GetComponent<PlayerInteract>();
            Inventory = GetComponent<PlayerInventory>();
            
            _playerMapLimitBoundaries = new PlayerMapLimitBoundaries(transform, -25, 25, -25, 25);
        }

        private void Update()
        {
            _playerLocomotion.HandleAllMovement();
            _playerInteract.TryInteract();
            _playerMapLimitBoundaries.KeepWithinBoundaries();
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
