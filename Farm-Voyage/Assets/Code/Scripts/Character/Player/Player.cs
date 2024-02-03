using InputManagers;
using UnityEngine;
using Zenject;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerLocomotion))]
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour
    {
        public PlayerInput Input { get; private set; }
         
        private PlayerLocomotion _playerLocomotion;

        [Inject]
        private void Construct(PlayerInput playerInput)
        {
            Input = playerInput;
        }
        
        private void Awake()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
        }

        private void Update()
        {
            _playerLocomotion.HandleAllMovement();
        }
    }
}
