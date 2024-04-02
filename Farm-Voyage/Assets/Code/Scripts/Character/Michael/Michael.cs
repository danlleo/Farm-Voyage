using Character.Michael.Events;
using Character.Michael.Locomotion;
using Common;
using UnityEngine;
using StateFactory = Character.Michael.StateMachine.StateFactory;

namespace Character.Michael
{
    [DisallowMultipleComponent]
    public sealed class Michael : MonoBehaviour, IVisitable
    {
        public StateFactory StateFactory { get; private set; }
        public MichaelLocomotion Locomotion { get; private set; }
        public WaterStateObserver WaterStateObserver { get; private set; }
        public MichaelEvents Events { get; private set; }
        [field: SerializeField] public MichaelTransformPoints TransformPoints { get; private set; }

        private StateMachine.StateMachine _stateMachine;
        
        private void Awake()
        {
            Events = new MichaelEvents();
            
            Locomotion = GetComponent<MichaelLocomotion>();
            WaterStateObserver = GetComponent<WaterStateObserver>();
            
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
            _stateMachine.Initialize(StateFactory.Idle());
        }
        
        private void Update()
        {
            _stateMachine.CurrentState.Tick();
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
