using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] public AudioSource audioSource;
        [SerializeField] public AudioSource musicSource;
        [SerializeField] private AudioClip[] audioClips;
        private float _musicVolume = 1f;
        private float _soundVolume = 1f;

        public void Awake()
        {
            audioSource = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
            musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
        }

        public void Update()
        {
            musicSource.volume = _musicVolume;
            audioSource.volume = _soundVolume;
        }

        public void PlaySound(int clipNumber)
        {
            if(audioSource.enabled){
                audioSource.PlayOneShot(audioClips[clipNumber]);
            }
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;
        }
        public void SetSoundVolume(float volume)
        {
            _soundVolume = volume;
        }
    }
}
