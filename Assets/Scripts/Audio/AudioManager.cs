using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource; 
        public AudioSource musicSource;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private AudioClip[] audioClips;


        public void Awake()
        {
            audioSource = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
            musicSource = musicSource.GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (PlayerPrefs.HasKey("musicVol") && PlayerPrefs.HasKey("soundVol")) return;
            musicSource.volume = 1f;
            audioSource.volume = 1f;
        }

        public void Update()
        {
            musicSource.volume = PlayerPrefs.GetFloat("musicVol");
            musicSlider.value = PlayerPrefs.GetFloat("musicVol");
            audioSource.volume = PlayerPrefs.GetFloat("soundVol");
            soundSlider.value = PlayerPrefs.GetFloat("soundVol");
        }

        public void PlaySound(int clipNumber)
        {
            if(audioSource.enabled){
                audioSource.PlayOneShot(audioClips[clipNumber]);
            }
        }

        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat("musicVol", volume);
        }
        public void SetSoundVolume(float volume)
        {
            PlayerPrefs.SetFloat("soundVol", volume);
        }
    }
}
