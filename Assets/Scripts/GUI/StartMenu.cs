using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI
{
    public class StartMenu : MonoBehaviour
    {
        public static bool GamePaused = false;
        [SerializeField] private GameObject StartMenuUI;
        [SerializeField] private GameObject OptionsMenuUI;
        [SerializeField] private AudioSource buttonClick;

        void Start()
        {
            StartMenuUI.SetActive(true);
            OptionsMenuUI.SetActive(false);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void Menu()
        {
            StartMenuUI.SetActive(true);
            OptionsMenuUI.SetActive(false);
        }

        public void LoadOptions()
        {
            Debug.Log("Loading Options menu...");
            StartMenuUI.SetActive(false);
            OptionsMenuUI.SetActive(true);
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
