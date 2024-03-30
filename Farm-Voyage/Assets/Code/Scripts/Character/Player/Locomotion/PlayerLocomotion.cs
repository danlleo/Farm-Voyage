using System;
using System.Collections;
using UnityEngine;
using Utilities;

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
        [SerializeField, Range(0.1f, 5f)] private float _stickDistance = 3f;
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

        public void HandleMoveDestination(Transform destination, Quaternion endRotation, Action onFinishedMoving = null)
        {
            HandleMoveDestination(destination.position, endRotation, onFinishedMoving);
        }
        
        public void HandleMoveDestination(Vector3 destination, Quaternion endRotation, Action onFinishedMoving = null)
        {
            CoroutineHandler.StopCoroutine(this, _moveDestinationRoutine);
            CoroutineHandler.StartAndOverride(this, ref _moveDestinationRoutine,
                MoveDestinationRoutine(destination, endRotation, onFinishedMoving));
        }
        
        public void HandleRotation()
        {
            transform.forward = Vector3.Slerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
        }
        
        public void StartStickRotation(Transform lookTransform, float stickDistance, Action onOutOfZone = null)
        {
            if (stickDistance < 0f)
            {
                stickDistance = 0f;
                Debug.LogWarning("Stick distance is below zero, please take a look.");
            }

            CoroutineHandler.ReassignAndStart(this, ref _stickRotationRoutine,
                StickRotationRoutine(lookTransform, stickDistance, onOutOfZone));
        }
        
        public void StartStickRotation(Transform lookTransform, Action onOutOfZone = null)
        {
            StartStickRotation(lookTransform, _stickDistance, onOutOfZone);
        }
        
        public void StopAllMovement()
        {
            CoroutineHandler.StopCoroutine(this, _stickRotationRoutine);
            _player.PlayerEvents.PlayerIdleEvent.Call(this);
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
                _player.PlayerEvents.PlayerWalkingEvent.Call(this);
            }
            else
            {
                _player.PlayerEvents.PlayerIdleEvent.Call(this);
            }
        }
        
        private IEnumerator MoveDestinationRoutine(Vector3 targetPosition, Quaternion endRotation, Action onReachedDestination)
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
            onReachedDestination?.Invoke();

            InvokePlayersLocomotionEvents();
            HandleTargetRotation(endRotation);
            
            yield return null;
        }

        private IEnumerator StickRotationRoutine(Transform lookTransform, float stickDistance, Action onOutOfZone = null)
        {
            while (lookTransform != null)
            {
                if (!(Vector3.Distance(_player.transform.position, lookTransform.position) <=
                      stickDistance))
                {
                    break;
                }
                
                Vector3 lookDirection = lookTransform.position - transform.position;
                lookDirection.y = 0;
                
                if (lookDirection == Vector3.zero) continue;

                Vector3 forward = Vector3.Slerp(transform.forward, lookDirection.normalized,
                    Time.deltaTime * _stickRotateSpeed);
                
                transform.forward = forward;
             
                yield return null;
            }

            onOutOfZone?.Invoke();
        }
    }
}
