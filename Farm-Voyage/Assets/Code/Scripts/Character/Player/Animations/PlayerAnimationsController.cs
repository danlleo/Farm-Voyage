using System;
using Attributes.WithinParent;
using Character.Player.Events;
using Farm.FarmResources;
using UnityEngine;

namespace Character.Player.Animations
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class PlayerAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private ParticleSystem _walkingEffectParticleSystem;
        [SerializeField, WithinParent] private Player _player;
        
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _player.Events.IdleEvent.OnPlayerIdle += OnIdle;
            _player.Events.WalkingEvent.OnPlayerWalking += OnWalking;
            _player.Events.GatheringEvent.OnPlayerGathering += OnGathering;
            _player.Events.DiggingPlantAreaStateChangedEvent.OnPlayerDiggingPlantStateChangedArea +=
                OnDiggingPlantStateChanged;
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged +=
                OnCarryingBoxStateChanged;
            _player.Events.HarvestingStateChangedEvent.OnPlayerHarvestingStateChanged +=
                OnHarvestingStateChanged;
            _player.Events.WateringStateChangedEvent.OnPlayerWateringStateChanged +=
                OnWateringStateChanged;
        }

        private void OnDisable()
        {
            _player.Events.IdleEvent.OnPlayerIdle -= OnIdle;
            _player.Events.WalkingEvent.OnPlayerWalking -= OnWalking;
            _player.Events.GatheringEvent.OnPlayerGathering -= OnGathering;
            _player.Events.DiggingPlantAreaStateChangedEvent.OnPlayerDiggingPlantStateChangedArea -=
                OnDiggingPlantStateChanged;
            _player.Events.CarryingStorageBoxStateChangedEvent.OnPlayerCarryingStorageBoxStateChanged -=
                OnCarryingBoxStateChanged;
            _player.Events.HarvestingStateChangedEvent.OnPlayerHarvestingStateChanged -=
                OnHarvestingStateChanged;
            _player.Events.WateringStateChangedEvent.OnPlayerWateringStateChanged -=
                OnWateringStateChanged;
        }

        private void OnIdle(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, false);
            _walkingEffectParticleSystem.Stop();
        }

        private void OnWalking(object sender, EventArgs e)
        {
            _animator.SetBool(PlayerAnimationParams.IsWalking, true);
            
            if (!_walkingEffectParticleSystem.isPlaying)
                _walkingEffectParticleSystem.Play();
        }
        
        private void OnGathering(object sender, PlayerGatheringEventArgs e)
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
        
        private void OnDiggingPlantStateChanged(object sender, PlayerDiggingPlantAreaEventArgs e)
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

        private void OnCarryingBoxStateChanged(object sender,
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
        
        private void OnHarvestingStateChanged(bool isHarvesting)
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
        
        private void OnWateringStateChanged(bool isWatering)
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