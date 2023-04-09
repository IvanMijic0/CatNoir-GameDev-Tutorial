using UnityEngine;
using Audio;

namespace GUI
{
    public class MenuController : MonoBehaviour
    {
       
        [SerializeField] private GameObject pauseMenuUI;
        [SerializeField] private GameObject optionsMenuUI;
        [SerializeField] private AudioSource buttonClick;
        [SerializeField] private GameObject healthBar;

        private static bool _gamePaused;
        private PlayerController _playerController;

        private void Awake()
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        
        private void Update()
        {
            PressEscape();
        }


        private void PressEscape()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            if (_gamePaused)
                Resume();
            else
                Pause();
        }
        
        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
            healthBar.SetActive(false);
            DisableControl(_playerController, _playerController.GetProjectileFire(), _playerController.GetAudioManager());
            Time.timeScale = 0f;
            _gamePaused = true;
        }
        
        private void Resume()
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            healthBar.SetActive(true);
            EnableControl(_playerController, _playerController.GetProjectileFire(), _playerController.GetAudioManager());
            Time.timeScale = 1f;
            _gamePaused = false;
        }
        
        public void LoadOptions()
        {
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(true);
        }
        

        /*public void Restart(_audioManager audioManager)
        {
            SceneManager.LoadScene(0);
            pauseMenuUI.SetActive(false);
            audioManager.audioSource.enabled = true;
            Time.timeScale = 1f;
            _gamePaused = false;
        }*/

        public void SaveQuitGame()
        {
            Application.Quit();
        }

        public void ButtonClick()
        {
            buttonClick.Play();
        }
        
        private static void DisableControl(Behaviour playerController, Behaviour proj, AudioManager audioManager)
        {
            playerController.enabled = false;
            proj.enabled = false;
            audioManager.audioSource.enabled = false;
        }
        
        private static void EnableControl(Behaviour playerController, Behaviour proj, AudioManager audioManager)
        {
            playerController.enabled = true;
            proj.enabled = true;
            audioManager.audioSource.enabled = true;
        }
    }
}
