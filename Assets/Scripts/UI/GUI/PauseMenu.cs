using UnityEngine;
using UnityEngine.SceneManagement;
using Behaviours.Combat.Player;
using Audio;
using Behaviours.Movement.PlayerMovement;

namespace GUI
{
    public class PauseMenu : MonoBehaviour
    {
       
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private GameObject optionsMenuUI;
        [SerializeField] private AudioSource buttonClick;
        [SerializeField] private PlayerMovement playerMove;
        [SerializeField] private PlayerProjectileFire proj;

        private static bool _gamePaused;
        private AudioManager _audioManager;
        private AudioSource _sound;

        void Awake()
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            GameObject.Find("Music").GetComponent<AudioSource>();
            playerMove = GameObject.Find("Player").GetComponent<PlayerMovement>();
            proj = GameObject.Find("Player").GetComponent<PlayerProjectileFire>();
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        public void Update() 
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(_gamePaused){
                    Resume();
                }
                else{
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            playerMove.enabled = true;
            proj.enabled = true;
            _audioManager.audioSource.enabled = true;
            Time.timeScale = 1f;
            _gamePaused = false;
        }

        public void Pause()
        {
            pauseMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
            playerMove.enabled = false;
            proj.enabled = false;
            _audioManager.audioSource.enabled = false;
            Time.timeScale = 0f;
            _gamePaused = true;
        }

        public void LoadOptions()
        {
            Debug.Log("Loading Options menu...");
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            pauseMenuUI.SetActive(false);
            _audioManager.audioSource.enabled = true;
            Time.timeScale = 1f;
            _gamePaused = false;
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
