using UnityEngine;

namespace Sound
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip Clip;
        
        [Range(0f, 1f)] public float Volume = 0.7f;
        [Range(0.5f, 1.5f)] public float Pitch = 1f;
        [Range(0f, 0.5f)] public float RandomVolume = 0.1f;
        [Range(0f, 0.5f)] public float RandomPitch = 0.1f;

        private AudioSource _source;

        public void SetSource(AudioSource source)
        {
            _source = source;
            _source.clip = Clip;
        }

        public void Play()
        {
            _source.volume = Volume * (1 + Random.Range(-RandomVolume / 2f, RandomVolume / 2f));
            _source.pitch = Pitch * (1 + Random.Range(-RandomPitch / 2f, RandomPitch / 2f));
            _source.Play();
        }
    }
}