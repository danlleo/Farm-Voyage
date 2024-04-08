using Misc;
using UnityEngine;
using Utilities;

namespace Sound
{
    public class SoundFXManager : Singleton<SoundFXManager>
    {
        [Header("External references")]
        [SerializeField] private AudioSource _soundFXObject;
        
        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
        {
            AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;

            Destroy(audioSource.gameObject, clipLength);
        }
        
        public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume = 1f)
        {
            AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClips.GetRandomItem();
            audioSource.volume = volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;

            Destroy(audioSource.gameObject, clipLength);
        }
    }
}