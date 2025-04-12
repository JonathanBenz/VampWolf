using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampwolf
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject mainMenuPanel;
        public GameObject optionsPanel;
        public GameObject controlsPanel;
        
        // Start is called before the first frame update
        void Start()
        {
            //Make sure the main menu is the first screen
            mainMenuPanel.SetActive(true);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        //Open the options menu
        public void OpenOptions()
        {
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            controlsPanel.SetActive(false);
        }

        //Close the options menu
        public void CloseOptions()
        {
            mainMenuPanel.SetActive(true);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        //Open the controls menu
        public void OpenControls()
        {
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(true);
        }

        //Close the controls menu
        public void CloseControls()
        {
            //Closing the controls menu will take you back to options
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            controlsPanel.SetActive(false);
        }

    }
}
