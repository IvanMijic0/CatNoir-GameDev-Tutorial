using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Behaviours.Combat.Player;
using Behaviours.Movement;
using Audio;

namespace GUI
{
    public class PauseMenu : MonoBehaviour
    {
       
        [SerializeField] private GameObject PauseMenuUI;
        [SerializeField] private GameObject OptionsMenuUI;
        [SerializeField] private AudioSource buttonClick;
        [SerializeField] private PlayerMovement playerMove;
        [SerializeField] private PlayerProjectileFire proj;
        
        public static bool GamePaused = false;
        private AudioManager audioManager;
        private AudioSource music;
        private AudioSource sound;

        void Awake()
        {
            PauseMenuUI.SetActive(false);
            OptionsMenuUI.SetActive(false);
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            music = GameObject.Find("Music").GetComponent<AudioSource>();
            playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
            proj = GameObject.Find("Player").GetComponent<PlayerProjectileFire>();
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public void Update() 
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(GamePaused){
                    Resume();
                }
                else{
                    Pause();
                }
            }
        }

        public void Resume()
        {
            PauseMenuUI.SetActive(false);
            OptionsMenuUI.SetActive(false);
            playerMove.enabled = true;
            proj.enabled = true;
            audioManager.audioSource.enabled = true;
            Time.timeScale = 1f;
            GamePaused = false;
        }

        public void Pause()
        {
            PauseMenuUI.SetActive(true);
            OptionsMenuUI.SetActive(false);
            playerMove.enabled = false;
            proj.enabled = false;
            audioManager.audioSource.enabled = false;
            Time.timeScale = 0f;
            GamePaused = true;
        }

        public void LoadOptions()
        {
            Debug.Log("Loading Options menu...");
            PauseMenuUI.SetActive(false);
            OptionsMenuUI.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PauseMenuUI.SetActive(false);
            audioManager.audioSource.enabled = true;
            Time.timeScale = 1f;
            GamePaused = false;
        }

        public void SaveQuitGame()
        {
            Debug.Log("Quitting game...");
            SceneManager.LoadScene(0);
        }

        public void ButtonClick()
        {
            buttonClick.Play();
        }
    }
}
