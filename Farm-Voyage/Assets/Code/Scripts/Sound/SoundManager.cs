using UnityEngine;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        
        [SerializeField] private Sound[] _effects;
        [SerializeField] private Sound[] _musicTracks;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one SoundManager in the scene.");
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            InitializeSounds(_effects);
            InitializeSounds(_musicTracks);

            PlayMusic(MusicTrack.MainTheme);
        }

        private void InitializeSounds(Sound[] sounds)
        {
            foreach (Sound sound in sounds)
            {
                GameObject soundObject = new GameObject("Sound_" + sound.Clip.name);
                soundObject.transform.SetParent(this.transform);
                sound.SetSource(soundObject.AddComponent<AudioSource>());
            }
        }

        public void PlaySound(SoundEffect effect)
        {
            _effects[(int)effect].Play();
        }

        public void PlayMusic(MusicTrack track)
        {
            _musicTracks[(int)track].Play();
        }
    }
}