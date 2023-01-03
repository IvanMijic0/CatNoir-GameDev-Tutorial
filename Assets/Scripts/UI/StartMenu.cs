using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private GameObject startMenuUI;
        [SerializeField] private GameObject optionsMenuUI;
        [SerializeField] private AudioSource buttonClick;

        private void Start()
        {
            startMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void Menu()
        {
            startMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
        }

        public void LoadOptions()
        {
            Debug.Log("Loading Options menu...");
            startMenuUI.SetActive(false);
            optionsMenuUI.SetActive(true);
        }

        public void SaveQuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }

        public void ButtonClick()
        {
            buttonClick.Play();
        }
    }
}
