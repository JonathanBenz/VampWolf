using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pausePanel;
        public GameObject optionsPanel;
        public GameObject controlsPanel;

        private bool isPaused = false;

        // Start is called before the first frame update
        void Start()
        {
            //Make sure the game is the screen showing
            pausePanel.SetActive(false);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);

            //Make sure the game is playing
            Time.timeScale = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            //Pauses the game when the player presses esc
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Pause()
        {
            isPaused = true;
            Time.timeScale = 0f; // Freeze the game
            pausePanel.SetActive(true);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        public void Resume()
        {
            isPaused = false;
            Time.timeScale = 1f; // Unfreeze the game
            pausePanel.SetActive(false);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        //Open the options menu
        public void OpenOptions()
        {
            pausePanel.SetActive(false);
            optionsPanel.SetActive(true);
            controlsPanel.SetActive(false);
        }

        //Close the options menu
        public void CloseOptions()
        {
            pausePanel.SetActive(true);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        //Open the controls menu
        public void OpenControls()
        {
            pausePanel.SetActive(false);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(true);
        }

        //Close the controls menu
        public void CloseControls()
        {
            //Closing the controls menu will take you back to options
            pausePanel.SetActive(false);
            optionsPanel.SetActive(true);
            controlsPanel.SetActive(false);
        }
    }
}
