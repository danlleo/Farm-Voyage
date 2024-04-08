using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    [DisallowMultipleComponent]
    public class SoundMixerManager : MonoBehaviour
    {
        [Header("External references")]
        [SerializeField] private AudioMixer _audioMixer;

        public void SetMasterVolume(float level)
        {
            _audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
        }

        public void SetSoundFXVolume(float level)
        {
            _audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
        }

        public void SetMusicVolume(float level)
        {
            _audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
        }
    }
}
