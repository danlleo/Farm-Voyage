using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _obstaclesLayerMask;
        [SerializeField, Range(0f, 0.5f)] private float _walkingSpeed;
        [SerializeField, Range(0f, 0.5f)] private float _runningSpeed;
        [SerializeField, Range(0f, 20f)] private float _rotateSpeed;
        
        private Player _player;

        private Vector3 _moveDirection;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
        }

        public void StopAllMovement()
        {
            _player.PlayerIdleEvent.Call(this);
        }
        
        private void HandleGroundedMovement()
        {
            _moveDirection = new Vector3(_player.Input.MovementInput.x, 0f, _player.Input.MovementInput.y) * -1f;

            float moveDistance = Time.deltaTime * _player.Input.MoveAmount <= 0.5f ? _walkingSpeed : _runningSpeed;
            
            Vector3 playerBottomPoint = transform.position;
            Vector3 playerTopPoint = playerBottomPoint + Vector3.up * Player.Height;
            
            bool canMove = !Physics.CapsuleCast(playerBottomPoint, playerTopPoint, Player.Radius, _moveDirection,
                moveDistance, _obstaclesLayerMask);

            if (!canMove)
            {
                Vector3 moveDirectionX = new Vector3(_moveDirection.x, 0f, 0f).normalized;

                canMove = _moveDirection.x != 0 && !Physics.CapsuleCast(playerBottomPoint, playerTopPoint,
                    Player.Radius, moveDirectionX, moveDistance);

                if (canMove)
                {
                    _moveDirection = moveDirectionX;
                }
                else
                {
                    var moveDirectionZ = new Vector3(0f, 0f, _moveDirection.z);
                    
                    canMove = _moveDirection.z != 0 && !Physics.CapsuleCast(playerBottomPoint, playerTopPoint,
                        Player.Radius, moveDirectionZ, moveDistance);

                    if (canMove)
                        _moveDirection = moveDirectionZ;
                }
            }

            if (canMove)
            {
                transform.position += moveDistance * _moveDirection;
            }

            if (_moveDirection != Vector3.zero)
            {
                _player.PlayerWalkingEvent.Call(this);
            }
            else
            {
                _player.PlayerIdleEvent.Call(this);
            }
        }

        private void HandleRotation()
        {
            transform.forward = Vector3.Slerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }
    }
}
