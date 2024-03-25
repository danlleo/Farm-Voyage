using Character.Michael.Locomotion;
using Farm.Plants;
using UnityEngine;
using StateFactory = Character.Michael.StateMachine.StateFactory;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelLocomotionStateChangedEvent))]
    [RequireComponent(typeof(MichaelWateringPlantEvent))]
    [RequireComponent(typeof(MichaelHarvestingPlantEvent))]
    [DisallowMultipleComponent]
    public sealed class Michael : MonoBehaviour, ICharacter
    {
        public StateFactory StateFactory { get; private set; }
        
        public MichaelLocomotionStateChangedEvent MichaelLocomotionStateChangedEvent { get; private set; }
        public MichaelWateringPlantEvent MichaelWateringPlantEvent { get; private set; }
        public MichaelHarvestingPlantEvent MichaelHarvestingPlantEvent { get; private set; }
        
        public MichaelLocomotion MichaelLocomotion { get; private set; }
        public WaterStateObserver WaterStateObserver { get; private set; }
        
        private StateMachine.StateMachine _stateMachine;
        
        private void Awake()
        {
            MichaelLocomotionStateChangedEvent = GetComponent<MichaelLocomotionStateChangedEvent>();
            MichaelWateringPlantEvent = GetComponent<MichaelWateringPlantEvent>();
            MichaelHarvestingPlantEvent = GetComponent<MichaelHarvestingPlantEvent>();
            
            MichaelLocomotion = GetComponent<MichaelLocomotion>();
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

        public void Accept(IPlantVisitor plantVisitor)
        {
            plantVisitor.Visit(this);
        }
    }
}
