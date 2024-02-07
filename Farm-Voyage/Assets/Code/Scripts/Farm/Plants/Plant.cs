using Common;
using UnityEngine;

namespace Farm.Plants
{
    [DisallowMultipleComponent]
    public class Plant : MonoBehaviour, IInteractable
    {
        public StateMachine StateMachine { get; private set; }
        public StateFactory StateFactory { get; private set; }    
        
        public Vector3 CurrentScale => transform.localScale;
        public Vector3 TargetScale => _grownScale * Vector3.one;
        
        [Header("Settings")]
        [SerializeField, Range(0.1f, 3f)] public float _grownScale;
        [SerializeField, Range(0.1f, 3f)] private float _initialScale; 
        [SerializeField, Range(1f, 60f)] private float _growTimeInSeconds;

        private void Awake()
        {
            StateMachine = new StateMachine();
            StateFactory = new StateFactory(this, StateMachine);
        }

        private void Start()
        {
            StateMachine.Initialize(StateFactory.Growing());
        }

        private void Update()
        {
            StateMachine.CurrentState.Tick();
        }

        public void Interact()
        {
            
        }

        public void StopInteract()
        {
            
        }
    }
}
