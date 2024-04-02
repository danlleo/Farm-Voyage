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
            _michael.Events.MichaelLocomotionStateChangedEvent.OnMichaelWalking +=
                Michael_OnMichaelLocomotionStateChanged;
            _michael.Events.MichaelWateringPlantEvent.OnMichaelWateringPlant += Michael_OnMichaelWateringPlant;
            _michael.Events.MichaelHarvestingPlantEvent.OnMichaelHarvestingPlant += Michael_OnMichaelHarvestingPlant;
            _michael.Events.MichaelPerformingGardeningActionEvent.OnMichaelPerformingGardeningAction +=
                Michael_OnMichaelPerformingGardeningAction;
            _michael.Events.MichaelSittingStateChangedEvent.OnMichaelSittingStateChanged +=
                Michael_OnMichaelSittingStateChanged;
        }

        private void OnDisable()
        {
            _michael.Events.MichaelLocomotionStateChangedEvent.OnMichaelWalking -=
                Michael_OnMichaelLocomotionStateChanged;
            _michael.Events.MichaelWateringPlantEvent.OnMichaelWateringPlant -= Michael_OnMichaelWateringPlant;
            _michael.Events.MichaelHarvestingPlantEvent.OnMichaelHarvestingPlant -= Michael_OnMichaelHarvestingPlant;
            _michael.Events.MichaelPerformingGardeningActionEvent.OnMichaelPerformingGardeningAction -=
                Michael_OnMichaelPerformingGardeningAction;
            _michael.Events.MichaelSittingStateChangedEvent.OnMichaelSittingStateChanged -=
                Michael_OnMichaelSittingStateChanged;
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