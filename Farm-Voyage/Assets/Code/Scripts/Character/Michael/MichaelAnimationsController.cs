using Character.Michael.Locomotion;
using UnityEngine;

namespace Character.Michael
{
    [RequireComponent(typeof(MichaelLocomotionStateChangedEvent))]
    [RequireComponent(typeof(MichaelWateringPlantEvent))]
    [RequireComponent(typeof(MichaelHarvestingPlantEvent))]
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class MichaelAnimationsController : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private ParticleSystem _walkingEffectParticleSystem;
        
        private MichaelLocomotionStateChangedEvent _michaelLocomotionStateChangedEvent;
        private MichaelWateringPlantEvent _michaelWateringPlantEvent;
        private MichaelHarvestingPlantEvent _michaelHarvestingPlantEvent;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _michaelLocomotionStateChangedEvent = GetComponent<MichaelLocomotionStateChangedEvent>();
            _michaelWateringPlantEvent = GetComponent<MichaelWateringPlantEvent>();
            _michaelHarvestingPlantEvent = GetComponent<MichaelHarvestingPlantEvent>();
        }

        private void OnEnable()
        {
            _michaelLocomotionStateChangedEvent.OnMichaelWalking += Michael_OnMichaelLocomotionStateChanged;
            _michaelWateringPlantEvent.OnMichaelWateringPlant += Michael_OnMichaelWateringPlant;
            _michaelHarvestingPlantEvent.OnMichaelHarvestingPlant += Michael_OnMichaelHarvestingPlant;
        }

        private void OnDisable()
        {
            _michaelLocomotionStateChangedEvent.OnMichaelWalking -= Michael_OnMichaelLocomotionStateChanged;
            _michaelWateringPlantEvent.OnMichaelWateringPlant -= Michael_OnMichaelWateringPlant;
        }

        private void Michael_OnMichaelLocomotionStateChanged(bool isWalking)
        {
            if (isWalking)
            {
                if (!_walkingEffectParticleSystem.isPlaying)
                    _walkingEffectParticleSystem.Play();
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
    }
}