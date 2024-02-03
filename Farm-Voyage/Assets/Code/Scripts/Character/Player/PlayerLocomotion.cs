using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerLocomotion : MonoBehaviour
    {
        [HideInInspector] public float VerticalMovement;
        [HideInInspector] public float HorizontalMovement;
        [HideInInspector] public float MoveAmount;

        [SerializeField] private float _walkingSpeed;
        [SerializeField] private float _runningSpeed;
        
        private Player _player;

        private Vector3 _moveDirection;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
        }

        private void HandleGroundedMovement()
        {
            // TODO: Ground movement
        }
    }
}
