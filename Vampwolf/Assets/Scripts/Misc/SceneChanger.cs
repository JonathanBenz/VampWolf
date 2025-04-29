using UnityEngine;
using UnityEngine.SceneManagement;
using Vampwolf;
public class SceneChanger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    /// <summary>
    /// Make sure this matches the Build Settings!
    /// </summary>
    enum levelNames
    {   //   0      1      2       3       4         5
        MAIN_MENU, HUB, FOREST, CASTLE, VILLAGE, LOSE_SCREEN
    }
    //Loads the Main Menu scene
    public void LoadMenu()
    {
        SceneManager.LoadScene((int)levelNames.MAIN_MENU);
    }

    //Loads the main hub/overworld/shop game scene
    public void LoadHub()
    {
        SceneManager.LoadScene((int)levelNames.HUB);
    }

    //Loads the Forest/Thicket game scene
    public void LoadForest()
    {
            SceneManager.LoadScene((int)levelNames.FOREST);
    }

    //Loads the Mansion/Castle game scene
    public void LoadCastle()
    {
            SceneManager.LoadScene((int)levelNames.CASTLE); 
    }

    //Loads the Village game scene
    public void LoadVillage()
    {
            SceneManager.LoadScene((int)levelNames.VILLAGE); 
    }


    //Loads the Lose Game scene
    public void LoadLoseScreen()
    {
        // Reset progress when loading into the lose screen
        ProgressTracker.Instance.level1Complete = false;
        ProgressTracker.Instance.level2Complete = false;

        SceneManager.LoadScene((int)levelNames.LOSE_SCREEN);
    }

    //Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }

   /* //Loads the Win Game scene
    public void LoadWinScreen()
    {
        SceneManager.LoadScene("Win Screen");
    }*/
}