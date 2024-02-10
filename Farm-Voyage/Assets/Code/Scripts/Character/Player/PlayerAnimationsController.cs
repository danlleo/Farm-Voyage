using System;
using Farm;
using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerGatheringEvent))]
    [RequireComponent(typeof(PlayerDiggingPlantAreaEvent))]
    [RequireComponent(typeof(PlayerCarryingStorageBoxStateChangedEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class PlayerAnimationsController : MonoBehaviour
    {
        private PlayerWalkingEvent _playerWalkingEvent;
        private PlayerIdleEvent _playerIdleEvent;
        private PlayerGatheringEvent _playerGatheringEvent;
        private PlayerDiggingPlantAreaEvent _playerDiggingPlantAreaEvent;
        private PlayerCarryingStorageBoxStateChangedEvent _playerCarryingStorageBoxStateChangedEvent;
        
        private Animator _animator;
        
        private void Awake()
        {
            _playerIdleEvent = GetComponent<PlayerIdleEvent>();
            _playerWalkingEvent = GetComponent<PlayerWalkingEvent>();
            _playerGatheringEvent = GetComponent<PlayerGatheringEvent>();
            _playerDiggingPlantAreaEvent = GetComponent<PlayerDiggingPlantAreaEvent>();
            _playerCarryingStorageBoxStateChangedEvent = GetComponent<PlayerCarryingStorageBoxStateChangedEvent>();
            
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _playerIdleEvent.OnPlayerIdle += PlayerIdleEvent_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking += PlayerWalkingEvent_OnPlayerWalking;
            _playerGatheringEvent.OnPlayerGathering += PlayerGatheringEvent_OnPlayerGathering;
            _playerDiggingPlantAreaEvent.OnPlayerDiggingPlantArea += PlayerDiggingPlantAreaEvent_OnPlayerDiggingPlantArea;
            _playerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged += PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingBoxStateChanged;
        }

        private void OnDisable()
        {
            _playerIdleEvent.OnPlayerIdle -= PlayerIdleEvent_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking -= PlayerWalkingEvent_OnPlayerWalking;
            _playerGatheringEvent.OnPlayerGathering -= PlayerGatheringEvent_OnPlayerGathering;
            _playerDiggingPlantAreaEvent.OnPlayerDiggingPlantArea -= PlayerDiggingPlantAreaEvent_OnPlayerDiggingPlantArea;
            _playerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -= PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingBoxStateChanged;
        }

        private void PlayerIdleEvent_OnPlayerIdle(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, false);
        }

        private void PlayerWalkingEvent_OnPlayerWalking(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, true);
        }
        
        private void PlayerGatheringEvent_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            _animator.speed = e.GatheringSpeed;
            
            switch (e.ResourceType)
            {
                case ResourceType.Rock:
                    _animator.SetBool(PlayerAnimationParams.IsMining, e.IsGathering);
                    break;
                case ResourceType.Wood:
                    break;
                case ResourceType.Dirt:
                    _animator.SetBool(PlayerAnimationParams.IsDigging, e.IsGathering);
                    break;
                case ResourceType.Water:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PlayerDiggingPlantAreaEvent_OnPlayerDiggingPlantArea(object sender, PlayerDiggingPlantAreaEventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsDigging, e.IsDigging);
        }

        private void PlayerCarryingStorageBoxStateChangedEvent_OnPlayerCarryingBoxStateChanged(object sender,
            PlayerCarryingStorageBoxStateChangedEventArgs e)
        {
            int carryingAnimationLayer = _animator.GetLayerIndex(PlayerAnimationLayers.Carrying);
            
            if (e.IsCarrying)
            {
                _animator.SetLayerWeight(carryingAnimationLayer, 1f);
                return;
            }
            
            _animator.SetLayerWeight(carryingAnimationLayer, 0f);
        }
    }
}