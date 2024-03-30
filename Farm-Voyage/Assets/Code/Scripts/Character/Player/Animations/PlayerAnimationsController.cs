using System;
using Character.Player.Locomotion;
using Farm.FarmResources;
using UnityEngine;

namespace Character.Player.Animations
{
    [RequireComponent(typeof(PlayerWalkingEvent))]
    [RequireComponent(typeof(PlayerIdleEvent))]
    [RequireComponent(typeof(PlayerGatheringEvent))]
    [RequireComponent(typeof(PlayerDiggingPlantAreaStateChangedEvent))]
    [RequireComponent(typeof(PlayerCarryingStorageBoxStateChangedEvent))]
    [RequireComponent(typeof(PlayerWateringStateChangedEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class PlayerAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private ParticleSystem _walkingEffectParticleSystem;
        
        private PlayerWalkingEvent _playerWalkingEvent;
        private PlayerIdleEvent _playerIdleEvent;
        private PlayerGatheringEvent _playerGatheringEvent;
        private PlayerDiggingPlantAreaStateChangedEvent _playerDiggingPlantAreaStateChangedEvent;
        private PlayerCarryingStorageBoxStateChangedEvent _playerCarryingStorageBoxStateChangedEvent;
        private PlayerHarvestingStateChangedEvent _playerHarvestingStateChangedEvent;
        private PlayerWateringStateChangedEvent _playerWateringStateChangedEvent;
        
        private Animator _animator;
        
        private void Awake()
        {
            _playerIdleEvent = GetComponent<PlayerIdleEvent>();
            _playerWalkingEvent = GetComponent<PlayerWalkingEvent>();
            _playerGatheringEvent = GetComponent<PlayerGatheringEvent>();
            _playerDiggingPlantAreaStateChangedEvent = GetComponent<PlayerDiggingPlantAreaStateChangedEvent>();
            _playerCarryingStorageBoxStateChangedEvent = GetComponent<PlayerCarryingStorageBoxStateChangedEvent>();
            _playerHarvestingStateChangedEvent = GetComponent<PlayerHarvestingStateChangedEvent>();
            _playerWateringStateChangedEvent = GetComponent<PlayerWateringStateChangedEvent>();
            
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _playerIdleEvent.OnPlayerIdle += Player_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking += Player_OnPlayerWalking;
            _playerGatheringEvent.OnPlayerGathering += Player_OnPlayerGathering;
            _playerDiggingPlantAreaStateChangedEvent.OnPlayerDiggingPlantStateChangedArea +=
                Player_OnPlayerDiggingPlantStateChanged;
            _playerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                Player_OnPlayerCarryingBoxStateChanged;
            _playerHarvestingStateChangedEvent.OnPlayerHarvestingStateChanged += Player_OnPlayerHarvestingStateChanged;
            _playerWateringStateChangedEvent.OnPlayerWateringStateChanged += Player_OnPlayerWateringStateChanged;
        }

        private void OnDisable()
        {
            _playerIdleEvent.OnPlayerIdle -= Player_OnPlayerIdle;
            _playerWalkingEvent.OnPlayerWalking -= Player_OnPlayerWalking;
            _playerGatheringEvent.OnPlayerGathering -= Player_OnPlayerGathering;
            _playerDiggingPlantAreaStateChangedEvent.OnPlayerDiggingPlantStateChangedArea -=
                Player_OnPlayerDiggingPlantStateChanged;
            _playerCarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                Player_OnPlayerCarryingBoxStateChanged;
            _playerHarvestingStateChangedEvent.OnPlayerHarvestingStateChanged -= Player_OnPlayerHarvestingStateChanged;
            _playerWateringStateChangedEvent.OnPlayerWateringStateChanged -= Player_OnPlayerWateringStateChanged;
        }

        private void Player_OnPlayerIdle(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, false);
            _walkingEffectParticleSystem.Stop();
        }

        private void Player_OnPlayerWalking(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, true);
            
            if (!_walkingEffectParticleSystem.isPlaying)
                _walkingEffectParticleSystem.Play();
        }
        
        private void Player_OnPlayerGathering(object sender, PlayerGatheringEventArgs e)
        {
            int gatheringAnimationLayer = _animator.GetLayerIndex(PlayerAnimationLayers.Gathering);
            
            _animator.SetLayerWeight(gatheringAnimationLayer, e.IsGathering ? 1f : 0f);
            _animator.speed = e.GatheringSpeed;
            
            switch (e.ResourceType)
            {
                case ResourceType.Rock:
                    _animator.SetBool(PlayerAnimationParams.IsMining, e.IsGathering);
                    break;
                case ResourceType.Wood:
                    _animator.SetBool(PlayerAnimationParams.IsChopping, e.IsGathering);
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
        
        private void Player_OnPlayerDiggingPlantStateChanged(object sender, PlayerDiggingPlantAreaEventArgs e)
        {
            int farmingAnimationLayer = _animator.GetLayerIndex(PlayerAnimationLayers.Farming);
            
            if (e.IsDigging)
            {
                _animator.SetLayerWeight(farmingAnimationLayer, 1f);
                _animator.SetBool(PlayerAnimationParams.IsDigging, e.IsDigging);
                return;
            }
            
            _animator.SetLayerWeight(farmingAnimationLayer, 0f);
            _animator.SetBool(PlayerAnimationParams.IsDigging, e.IsDigging);
        }

        private void Player_OnPlayerCarryingBoxStateChanged(object sender,
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
        
        private void Player_OnPlayerHarvestingStateChanged(bool isHarvesting)
        {
            int farmingAnimationLayer = _animator.GetLayerIndex(PlayerAnimationLayers.Farming);
            
            if (isHarvesting)
            {
                _animator.SetLayerWeight(farmingAnimationLayer, 1f);
                _animator.SetBool(PlayerAnimationParams.IsHarvesting, true);
                return;
            }
            
            _animator.SetLayerWeight(farmingAnimationLayer, 0f);
            _animator.SetBool(PlayerAnimationParams.IsHarvesting, false);
        }
        
        private void Player_OnPlayerWateringStateChanged(bool isWatering)
        {
            int farmingAnimationLayer = _animator.GetLayerIndex(PlayerAnimationLayers.Farming);
            
            if (isWatering)
            {
                _animator.SetLayerWeight(farmingAnimationLayer, 1f);
                _animator.SetBool(PlayerAnimationParams.IsWatering, true);
                return;
            }
            
            _animator.SetLayerWeight(farmingAnimationLayer, 0f);
            _animator.SetBool(PlayerAnimationParams.IsWatering, false);
        }
    }
}