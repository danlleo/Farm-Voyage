using Character.Michael.Locomotion;
using Character.Michael.StateMachine;
using UnityEngine;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelIdleEvent))]
    [RequireComponent(typeof(MichaelWalkingEvent))]
    [DisallowMultipleComponent]
    public sealed class Michael : MonoBehaviour
    {
        public StateFactory StateFactory { get; private set; }
        
        public MichaelIdleEvent MichaelIdleEvent { get; private set; }
        public MichaelWalkingEvent MichaelWalkingEvent { get; private set; }

        public MichaelLocomotion MichaelLocomotion { get; private set; }
        public Waterar Waterar { get; private set; }
        
        private StateMachine.StateMachine _stateMachine;
        
        private void Awake()
        {
            MichaelIdleEvent = GetComponent<MichaelIdleEvent>();
            MichaelWalkingEvent = GetComponent<MichaelWalkingEvent>();

            MichaelLocomotion = GetComponent<MichaelLocomotion>();
            Waterar = GetComponent<Waterar>();
            
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
            _stateMachine.Initialize(StateFactory.Gardening());
        }
        
        private void Update()
        {
            _stateMachine.CurrentState.Tick();
        }

        private void OnDestroy()
        {
            _stateMachine.CurrentState.OnExit();
        }
    }
}
