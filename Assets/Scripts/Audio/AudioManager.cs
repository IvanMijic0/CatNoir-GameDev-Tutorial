using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behaviours.Movement;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] public AudioSource audioSource;
        [SerializeField] public AudioSource musicSource;
        [SerializeField] private AudioClip[] audioClips;
        private float musicVolume = 1f;
        private float soundVolume = 1f;

        public void Awake()
        {
            audioSource = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
            musicSource = GameObject.Find("Music").GetComponent<AudioSource>();
        }

        public void Update()
        {
            musicSource.volume = musicVolume;
            audioSource.volume = soundVolume;
        }

        public void PlaySound(int clipNumber)
        {
            if(audioSource.enabled){
                audioSource.PlayOneShot(audioClips[clipNumber]);
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
        }
        public void SetSoundVolume(float volume)
        {
            soundVolume = volume;
        }
    }
}
