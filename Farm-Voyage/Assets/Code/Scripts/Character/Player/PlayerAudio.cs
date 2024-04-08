using Sound;
using UnityEngine;

namespace Character.Player
{
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class PlayerAudio : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private AudioClip _wateringAudioClip;
        [SerializeField] private AudioClip[] _stepsAudioClips;
        
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            _player.Events.WateringStateChangedEvent.OnPlayerWateringStateChanged +=
                WateringStateChangedEvent_OnPlayerWateringStateChanged;
        }

        private void OnDisable()
        {
            _player.Events.WateringStateChangedEvent.OnPlayerWateringStateChanged -=
                WateringStateChangedEvent_OnPlayerWateringStateChanged;
        }

        private void PlayRandomStepSound()
        {
            SoundFXManager.Instance.PlayRandomSoundFX3DClip(_stepsAudioClips, transform, 0.1f);
        }
        
        private void PlayWateringSound()
        {
            SoundFXManager.Instance.PlaySoundFX3DClip(_wateringAudioClip, transform, 0.3f);
        }
        
        private void WateringStateChangedEvent_OnPlayerWateringStateChanged(bool isWatering)
        {
            if (!isWatering) return;

            PlayWateringSound();
        }
    }
}