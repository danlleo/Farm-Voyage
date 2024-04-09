using System;
using Sound;
using UnityEngine;

namespace Character.Michael
{
    [RequireComponent(typeof(Michael))]
    [DisallowMultipleComponent]
    public class MichaelAudio : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private AudioClip _wateringAudioClip;
        [SerializeField] private AudioClip[] _stepsAudioClips;
        
        private Michael _michael;

        private void Awake()
        {
            _michael = GetComponent<Michael>();
        }

        private void OnEnable()
        {
            _michael.Events.WateringPlantEvent.OnMichaelWateringPlant += WateringPlantEvent_OnMichaelWateringPlant;
        }

        private void OnDisable()
        {
            _michael.Events.WateringPlantEvent.OnMichaelWateringPlant -= WateringPlantEvent_OnMichaelWateringPlant;
        }

        private void PlayRandomStepSound()
        {
            SoundFXManager.Instance.PlayRandomSoundFX3DClip(_stepsAudioClips, transform);
        }
        
        private void PlayWateringSound()
        {
            SoundFXManager.Instance.PlaySoundFX3DClip(_wateringAudioClip, transform);
        }
        
        private void WateringPlantEvent_OnMichaelWateringPlant(bool isWatering)
        {
            if (!isWatering) return;
            
            PlayWateringSound();
        }
    }
}
