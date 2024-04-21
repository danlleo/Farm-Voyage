using Character.Player.Events;
using Character.Player.Locomotion;
using Common;
using DataPersistence;
using InputManagers;
using UnityEngine;
using Zenject;
using StateFactory = Character.Player.StateMachine.StateFactory;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerInteract))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IVisitable
    {
        public const float Height = 1f;
        public const float Radius = .5f;

        public Stats Stats;
        
        public StateFactory StateFactory { get; private set; }
        public PlayerEvents Events { get; private set; }
        public PlayerInteract Interact { get; private set; }
        public PlayerLocomotion Locomotion { get; private set; }
        public IPlayerInput Input { get; private set; }
        [field: SerializeField] public PlayerTransformPoints TransformPoints { get; private set; }

        private StateMachine.StateMachine _stateMachine;
        
        private PlayerMapLimitBoundaries _playerMapLimitBoundaries;
        
        [Inject]
        private void Construct(IPlayerInput playerInput)
        {
            Input = playerInput;
        }
        
        private void Awake()
        {
            Events = new PlayerEvents();
            
            Locomotion = GetComponent<PlayerLocomotion>();
            Interact = GetComponent<PlayerInteract>();
            
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

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
