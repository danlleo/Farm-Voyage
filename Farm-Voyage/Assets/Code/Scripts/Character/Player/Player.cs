using Character.Player.Locomotion;
using Farm.Plants;
using InputManagers;
using UnityEngine;
using Zenject;
using StateFactory = Character.Player.StateMachine.StateFactory;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerInteract))]
    [RequireComponent(typeof(PlayerGatheringEvent))]
    [RequireComponent(typeof(PlayerCarryingStorageBoxStateChangedEvent))]
    [RequireComponent(typeof(PlayerFoundCollectableEvent))]
    [RequireComponent(typeof(PlayerShoppingEvent))]
    [RequireComponent(typeof(PlayerUsingWorkbenchEvent))]
    [RequireComponent(typeof(PlayerLeftHomeEvent))]
    [RequireComponent(typeof(PlayerEnteringHomeEvent))]
    [RequireComponent(typeof(PlayerExtractingWaterEvent))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, ICharacter
    {
        public const float Height = 1f;
        public const float Radius = .5f;
        
        public StateFactory StateFactory { get; private set; }
        
        public PlayerInteract PlayerInteract { get; private set; }
        public PlayerLocomotion PlayerLocomotion { get; private set; }
        public IPlayerInput Input { get; private set; }
        
        public PlayerWalkingEvent PlayerWalkingEvent { get; private set; }
        public PlayerIdleEvent PlayerIdleEvent { get; private set; }
        public PlayerGatheringEvent PlayerGatheringEvent { get; private set; }
        public PlayerDiggingPlantAreaEvent PlayerDiggingPlantAreaEvent { get; private set; }
        public PlayerCarryingStorageBoxStateChangedEvent PlayerCarryingStorageBoxStateChangedEvent { get; private set; }
        public PlayerFoundCollectableEvent PlayerFoundCollectableEvent { get; private set; }
        public PlayerShoppingEvent PlayerShoppingEvent { get; private set; }
        public PlayerUsingWorkbenchEvent PlayerUsingWorkbenchEvent { get; private set; }
        public PlayerLeftHomeEvent PlayerLeftHomeEvent { get; private set; }
        public PlayerEnteringHomeEvent PlayerEnteringHomeEvent { get; private set; }
        public PlayerExtractingWaterEvent PlayerExtractingWaterEvent { get; private set; }
        
        [field:SerializeField] public Transform WorkbenchStayPoint { get; private set; }
        [field:SerializeField] public Transform EmmaStoreStayPoint { get; private set; }
        [field:SerializeField] public Transform HomeLeavePoint { get; private set; }
        [field:SerializeField] public Transform HomeStayPoint { get; private set; }
        [field:SerializeField] public Transform WellStayPoint { get; private set; }
        
        private StateMachine.StateMachine _stateMachine;
        
        private PlayerMapLimitBoundaries _playerMapLimitBoundaries;
        
        [Inject]
        private void Construct(IPlayerInput playerInput)
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
            PlayerFoundCollectableEvent = GetComponent<PlayerFoundCollectableEvent>();
            PlayerShoppingEvent = GetComponent<PlayerShoppingEvent>();
            PlayerUsingWorkbenchEvent = GetComponent<PlayerUsingWorkbenchEvent>();
            PlayerLeftHomeEvent = GetComponent<PlayerLeftHomeEvent>();
            PlayerEnteringHomeEvent = GetComponent<PlayerEnteringHomeEvent>();
            PlayerExtractingWaterEvent = GetComponent<PlayerExtractingWaterEvent>();
            
            PlayerLocomotion = GetComponent<PlayerLocomotion>();
            PlayerInteract = GetComponent<PlayerInteract>();
            
            _playerMapLimitBoundaries = new PlayerMapLimitBoundaries(transform, -25, 25, -25, 25);
            _stateMachine = new StateMachine.StateMachine();
            StateFactory = new StateFactory(this, _stateMachine);
        }

        private void OnEnable()
        {
            _stateMachine.CurrentState?.SubscribeToEvents();
        }

        private void OnDisable()
        {
            _stateMachine.CurrentState.UnsubscribeFromEvents();
        }

        private void Start()
        {
            _stateMachine.Initialize(StateFactory.LeavingHome());
        }

        private void Update()
        {
            _stateMachine.CurrentState.Tick();
            _playerMapLimitBoundaries.KeepWithinBoundaries();
        }

        private void OnDestroy()
        {
            _stateMachine.CurrentState.OnExit();
        }

        public void Accept(IPlantVisitor plantVisitor)
        {
            plantVisitor.Visit(this);
        }
    }
}
