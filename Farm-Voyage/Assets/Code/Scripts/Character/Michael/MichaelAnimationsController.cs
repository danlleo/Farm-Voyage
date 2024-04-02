using System;
using System.Collections;
using UnityEngine;
using Utilities;

namespace Character.Michael
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class MichaelAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private ParticleSystem _walkingEffectParticleSystem;
        [SerializeField] private Michael _michael;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _michael.Events.LocomotionStateChangedEvent.OnMichaelWalking +=
                OnLocomotionStateChanged;
            _michael.Events.WateringPlantEvent.OnMichaelWateringPlant += OnWateringPlant;
            _michael.Events.HarvestingPlantEvent.OnMichaelHarvestingPlant += OnHarvestingPlant;
            _michael.Events.PerformingGardeningActionEvent.OnMichaelPerformingGardeningAction +=
                OnPerformingGardeningAction;
            _michael.Events.SittingStateChangedEvent.OnMichaelSittingStateChanged +=
                OnSittingStateChanged;
        }

        private void OnDisable()
        {
            _michael.Events.LocomotionStateChangedEvent.OnMichaelWalking -=
                OnLocomotionStateChanged;
            _michael.Events.WateringPlantEvent.OnMichaelWateringPlant -= OnWateringPlant;
            _michael.Events.HarvestingPlantEvent.OnMichaelHarvestingPlant -= OnHarvestingPlant;
            _michael.Events.PerformingGardeningActionEvent.OnMichaelPerformingGardeningAction -=
                OnPerformingGardeningAction;
            _michael.Events.SittingStateChangedEvent.OnMichaelSittingStateChanged -=
                OnSittingStateChanged;
        }

        private IEnumerator DelayAnimationActionRoutine(float delayTimeInSeconds, Action onAnimationFinished)
        {
            yield return new WaitForSeconds(delayTimeInSeconds);
            onAnimationFinished?.Invoke();
        }
        
        private void OnLocomotionStateChanged(bool isWalking)
        {
            if (isWalking)
            {
                if (!_walkingEffectParticleSystem.isPlaying)
                {
                    _walkingEffectParticleSystem.Play();
                }
            }
            else
            {
                _walkingEffectParticleSystem.Stop();
            }
            
            _animator.SetBool(MichaelAnimationsParams.IsWalking, isWalking);
        }
        
        private void OnWateringPlant(bool isWatering)
        {
            _animator.SetBool(MichaelAnimationsParams.IsWatering, isWatering);
        }
        
        private void OnHarvestingPlant(bool isHarvesting)
        {
            _animator.SetBool(MichaelAnimationsParams.IsHarvesting, isHarvesting);
        }

        private void OnPerformingGardeningAction(GardeningActionType gardeningActionType, Action onFinishedGardening)
        {
            string animationName = gardeningActionType switch
            {
                GardeningActionType.Looking => nameof(MichaelAnimationsParams.OnLooking),
                GardeningActionType.PickingUp => nameof(MichaelAnimationsParams.OnPicking),
                GardeningActionType.Plant => nameof(MichaelAnimationsParams.OnPlanting),
                GardeningActionType.Thinking => nameof(MichaelAnimationsParams.OnThinking),
                GardeningActionType.Angry => nameof(MichaelAnimationsParams.OnAngry),
                _ => throw new ArgumentOutOfRangeException(nameof(gardeningActionType), gardeningActionType, null)
            };

            _animator.SetTrigger(animationName);

            float animationLength = AnimatorUtils.GetAnimationClipLength(_animator, animationName);
            StartCoroutine(DelayAnimationActionRoutine(animationLength, onFinishedGardening));
        }
        
        private void OnSittingStateChanged(bool isSitting)
        {
            _animator.SetBool(MichaelAnimationsParams.IsSitting, isSitting);
        }
    }
}