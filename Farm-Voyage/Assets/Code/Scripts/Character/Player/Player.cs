using InputManagers;
using UnityEngine;
using Zenject;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerInteract))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour
    {
        public const float Height = 1f;
        public const float Radius = .5f;
     
        public PlayerWalkingEvent PlayerWalkingEvent { get; private set; }
        public PlayerIdleEvent PlayerIdleEvent { get; private set; }
        public PlayerInput Input { get; private set; }

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
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerInteract = GetComponent<PlayerInteract>();
        }

        private void Update()
        {
            _playerLocomotion.HandleAllMovement();
            _playerInteract.TryInteract();
        }
    }
}
