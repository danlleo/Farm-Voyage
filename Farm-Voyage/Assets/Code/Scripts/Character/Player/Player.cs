using System.Collections.Generic;
using Farm;
using InputManagers;
using UnityEngine;
using Zenject;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerLocomotion))]
    [RequireComponent(typeof(PlayerInteract))]
    [RequireComponent(typeof(PlayerGatheringEvent))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour
    {
        public const float Height = 1f;
        public const float Radius = .5f;
     
        public PlayerWalkingEvent PlayerWalkingEvent { get; private set; }
        public PlayerIdleEvent PlayerIdleEvent { get; private set; }
        public PlayerGatheringEvent PlayerGatheringEvent { get; private set; }
        public PlayerInput Input { get; private set; }
        public IEnumerable<Tool> ToolsList => _toolsList;
        
        private List<Tool> _toolsList = new()
        {
            new Tool(ToolType.Axe, 3f, 1),
            new Tool(ToolType.Pickaxe, 3f, 5),
            new Tool(ToolType.Shovel, 3f, 1),
        };
        
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
