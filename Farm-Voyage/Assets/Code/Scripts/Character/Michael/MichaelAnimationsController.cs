using System;
using System.Collections;
using Character.Michael.Locomotion;
using UnityEngine;
using Utilities;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelLocomotionStateChangedEvent))]
    [RequireComponent(typeof(MichaelWateringPlantEvent))]
    [RequireComponent(typeof(MichaelHarvestingPlantEvent))]
    [RequireComponent(typeof(MichaelPerformingGardeningActionEvent))]
    [RequireComponent(typeof(MichaelSittingStateChangedEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class MichaelAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private ParticleSystem _walkingEffectParticleSystem;
        
        private MichaelLocomotionStateChangedEvent _michaelLocomotionStateChangedEvent;
        private MichaelWateringPlantEvent _michaelWateringPlantEvent;
        private MichaelHarvestingPlantEvent _michaelHarvestingPlantEvent;
        private MichaelPerformingGardeningActionEvent _michaelPerformingGardeningActionEvent;
        private MichaelSittingStateChangedEvent _michaelSittingStateChangedEvent;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _michaelLocomotionStateChangedEvent = GetComponent<MichaelLocomotionStateChangedEvent>();
            _michaelWateringPlantEvent = GetComponent<MichaelWateringPlantEvent>();
            _michaelHarvestingPlantEvent = GetComponent<MichaelHarvestingPlantEvent>();
            _michaelPerformingGardeningActionEvent = GetComponent<MichaelPerformingGardeningActionEvent>();
            _michaelSittingStateChangedEvent = GetComponent<MichaelSittingStateChangedEvent>();
        }

        private void OnEnable()
        {
            _michaelLocomotionStateChangedEvent.OnMichaelWalking += Michael_OnMichaelLocomotionStateChanged;
            _michaelWateringPlantEvent.OnMichaelWateringPlant += Michael_OnMichaelWateringPlant;
            _michaelHarvestingPlantEvent.OnMichaelHarvestingPlant += Michael_OnMichaelHarvestingPlant;
            _michaelPerformingGardeningActionEvent.OnMichaelPerformingGardeningAction +=
                Michael_OnMichaelPerformingGardeningAction;
            _michaelSittingStateChangedEvent.OnMichaelSittingStateChanged += Michael_OnMichaelSittingStateChanged;
        }

        private void OnDisable()
        {
            _michaelLocomotionStateChangedEvent.OnMichaelWalking -= Michael_OnMichaelLocomotionStateChanged;
            _michaelWateringPlantEvent.OnMichaelWateringPlant -= Michael_OnMichaelWateringPlant;
            _michaelHarvestingPlantEvent.OnMichaelHarvestingPlant -= Michael_OnMichaelHarvestingPlant;
            _michaelPerformingGardeningActionEvent.OnMichaelPerformingGardeningAction -=
                Michael_OnMichaelPerformingGardeningAction;
            _michaelSittingStateChangedEvent.OnMichaelSittingStateChanged -= Michael_OnMichaelSittingStateChanged;
        }

        private IEnumerator DelayAnimationActionRoutine(float delayTimeInSeconds, Action onAnimationFinished)
        {
            yield return new WaitForSeconds(delayTimeInSeconds);
            onAnimationFinished?.Invoke();
        }
        
        private void Michael_OnMichaelLocomotionStateChanged(bool isWalking)
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
        
        private void Michael_OnMichaelWateringPlant(bool isWatering)
        {
            _animator.SetBool(MichaelAnimationsParams.IsWatering, isWatering);
        }
        
        private void Michael_OnMichaelHarvestingPlant(bool isHarvesting)
        {
            _animator.SetBool(MichaelAnimationsParams.IsHarvesting, isHarvesting);
        }

        private void Michael_OnMichaelPerformingGardeningAction(GardeningActionType gardeningActionType, Action onFinishedGardening)
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
        
        private void Michael_OnMichaelSittingStateChanged(bool isSitting)
        {
            _animator.SetBool(MichaelAnimationsParams.IsSitting, isSitting);
        }
    }
}