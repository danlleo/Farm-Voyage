using System;
using System.Collections;
using UnityEngine;

namespace Character.Player.Locomotion
{
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerLocomotion : MonoBehaviour
    {
        // TODO: make it visible in the inspector soon
        private const float StickDistance = 3f;
        
        [Header("Settings")]
        [SerializeField] private LayerMask _obstaclesLayerMask;
        [SerializeField, Range(1f, 100f)] private float _walkingSpeed;
        [SerializeField, Range(1f, 100f)] private float _runningSpeed;
        [SerializeField, Range(0f, 20f)] private float _rotateSpeed;
        [SerializeField, Range(0f, 50f)] private float _stickRotateSpeed;
        [SerializeField, Min(0f)] private float _stoppingDestinationDistance = 1f;

        private Player _player;
        private Transform _lockedObjectTransform;

        private Vector3 _moveDirection;

        private Coroutine _moveDestinationRoutine;
        private Coroutine _stickRotationRoutine;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
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
                Vector3 moveDirectionX = new(_moveDirection.x, 0f, 0f);
                
                if (moveDirectionX != Vector3.zero)
                {
                    canMove = !Physics.CapsuleCast(playerBottomPoint, playerTopPoint, Player.Radius, moveDirectionX.normalized,
                        out hit, moveDistance, _obstaclesLayerMask);
                    
                    if (canMove) _moveDirection = moveDirectionX.normalized;
                }

                if (!canMove)
                {
                    Vector3 moveDirectionZ = new(0f, 0f, _moveDirection.z);
                    
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

            InvokePlayersLocomotionEvents();
        }

        public void HandleMoveDestination(Vector3 targetPosition, Quaternion endRotation, Action onFinishedMoving = null)
        {
            if (_moveDestinationRoutine != null)
                StopCoroutine(_moveDestinationRoutine);

            _moveDestinationRoutine =
                StartCoroutine(MoveDestinationRoutine(targetPosition, endRotation, onFinishedMoving));
        }
        
        public void HandleRotation()
        {
            transform.forward = Vector3.Slerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }
        
        public void HandleStickRotation(Transform lookTransform, Action onOutOfZone = null)
        {
            if (_stickRotationRoutine != null)
                StopCoroutine(_stickRotationRoutine);

            _stickRotationRoutine = StartCoroutine(StickRotationRoutine(lookTransform, onOutOfZone));
        }
        
        public void StopAllMovement()
        {
            StopCoroutine(_moveDestinationRoutine);
            _player.PlayerIdleEvent.Call(this);
        }
        
        private void HandleTargetRotation(Quaternion rotation)
        {
            if (transform.rotation != rotation)
            {
                transform.forward =
                    Vector3.Slerp(transform.forward, rotation.eulerAngles, Time.deltaTime * _rotateSpeed);
            }
        }
        
        private void InvokePlayersLocomotionEvents()
        {
            if (_moveDirection != Vector3.zero)
            {
                _player.PlayerWalkingEvent.Call(this);
            }
            else
            {
                _player.PlayerIdleEvent.Call(this);
            }
        }
        
        private IEnumerator MoveDestinationRoutine(Vector3 targetPosition, Quaternion endRotation, Action onFinishedMoving)
        {
            while (!(Vector3.Distance(transform.position, targetPosition) <= _stoppingDestinationDistance))
            {
                InvokePlayersLocomotionEvents();
                HandleRotation();
                _moveDirection = targetPosition - transform.position;
                transform.position += _moveDirection.normalized * (_runningSpeed * Time.deltaTime);
                yield return null;
            }
            
            _moveDirection = Vector3.zero;
            onFinishedMoving?.Invoke();

            InvokePlayersLocomotionEvents();
            HandleTargetRotation(endRotation);
            
            yield return null;
        }

        private IEnumerator StickRotationRoutine(Transform lookTransform, Action onOutOfZone = null)
        {
            while (Vector3.Distance(_player.transform.position, lookTransform.position) <=
                StickDistance)
            {
                Vector3 lookDirection = lookTransform.position - transform.position;
                lookDirection.y = 0;

                if (lookDirection == Vector3.zero) yield break;
            
                Vector3 forward = Vector3.Slerp(transform.forward, lookDirection.normalized, Time.deltaTime * _stickRotateSpeed);
                transform.forward = forward;
             
                yield return null;
            }
            
            onOutOfZone?.Invoke();
        }
    }
}
