using UnityEngine;

namespace Character.Player.Locomotion
{
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerLocomotion : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _obstaclesLayerMask;
        [SerializeField, Range(1f, 100f)] private float _walkingSpeed;
        [SerializeField, Range(1f, 100f)] private float _runningSpeed;
        [SerializeField, Range(0f, 20f)] private float _rotateSpeed;
        [SerializeField, Range(0f, 50f)] private float _stickRotateSpeed;
        
        private Player _player;

        private Vector3 _moveDirection;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public void StopAllMovement()
        {
            _player.PlayerIdleEvent.Call(this);
        }
        
        public void HandleGroundedMovement()
        {
            _moveDirection = new Vector3(_player.Input.MovementInput.x, 0f, _player.Input.MovementInput.y) * -1f;

            float moveSpeed = _player.Input.MoveAmount <= 0.5f ? _walkingSpeed : _runningSpeed;
            float moveDistance = moveSpeed * Time.deltaTime;

            Vector3 playerBottomPoint = transform.position;
            Vector3 playerTopPoint = playerBottomPoint + Vector3.up * Player.Height;

            bool canMove = !Physics.CapsuleCast(playerBottomPoint, playerTopPoint, Player.Radius, _moveDirection.normalized,
                out RaycastHit hit, moveDistance, _obstaclesLayerMask);

            if (!canMove)
            {
                Vector3 moveDirectionX = new Vector3(_moveDirection.x, 0f, 0f);
                if (moveDirectionX != Vector3.zero)
                {
                    canMove = !Physics.CapsuleCast(playerBottomPoint, playerTopPoint, Player.Radius, moveDirectionX.normalized,
                        out hit, moveDistance, _obstaclesLayerMask);
                    if (canMove) _moveDirection = moveDirectionX.normalized;
                }

                if (!canMove)
                {
                    Vector3 moveDirectionZ = new Vector3(0f, 0f, _moveDirection.z);
                    if (moveDirectionZ != Vector3.zero)
                    {
                        canMove = !Physics.CapsuleCast(playerBottomPoint, playerTopPoint, Player.Radius, moveDirectionZ.normalized,
                            out hit, moveDistance, _obstaclesLayerMask);
                        if (canMove) _moveDirection = moveDirectionZ.normalized;
                    }
                }
            }

            // Move the player if no obstacles are detected
            if (canMove)
            {
                transform.position += _moveDirection.normalized * moveDistance;
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

        public void HandleRotation()
        {
            transform.forward = Vector3.Slerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }

        public void HandleStickRotation(Transform lookTransform)
        {
            Vector3 lookDirection = lookTransform.position - transform.position;
            lookDirection.y = 0;

            if (lookDirection == Vector3.zero) return;
            
            Vector3 forward = Vector3.Slerp(transform.forward, lookDirection.normalized, Time.deltaTime * _stickRotateSpeed);
            transform.forward = forward;
        }
    }
}
