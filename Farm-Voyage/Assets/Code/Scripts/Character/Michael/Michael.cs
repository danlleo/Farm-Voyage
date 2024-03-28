using Character.Michael.Locomotion;
using Farm.Plants;
using UnityEngine;
using StateFactory = Character.Michael.StateMachine.StateFactory;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelLocomotionStateChangedEvent))]
    [RequireComponent(typeof(MichaelWateringPlantEvent))]
    [RequireComponent(typeof(MichaelHarvestingPlantEvent))]
    [RequireComponent(typeof(MichaelPerformingGardeningActionEvent))]
    [DisallowMultipleComponent]
    public sealed class Michael : MonoBehaviour, ICharacter
    {
        public StateFactory StateFactory { get; private set; }
        public MichaelLocomotion MichaelLocomotion { get; private set; }
        public WaterStateObserver WaterStateObserver { get; private set; }
        public MichaelEvents MichaelEvents { get; private set; }
        [field: SerializeField] public MichaelTransformPoints TransformPoints { get; private set; }

        private StateMachine.StateMachine _stateMachine;
        
        private void Awake()
        {
            MichaelEvents = new MichaelEvents(
                GetComponent<MichaelLocomotionStateChangedEvent>(),
                GetComponent<MichaelWateringPlantEvent>(), 
                GetComponent<MichaelHarvestingPlantEvent>(),
                GetComponent<MichaelPerformingGardeningActionEvent>()   
            );
            
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

        public void Accept(IPlantVisitor plantVisitor)
        {
            plantVisitor.Visit(this);
        }
    }
}
