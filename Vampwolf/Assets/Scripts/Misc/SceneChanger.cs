using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Loads the Main Menu scene
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //Loads the main hub/overworld/shop game scene
    public void LoadHub()
    {
        SceneManager.LoadScene("Hub Scene");
    }

    //Loads the Forest/Thicket game scene
    public void LoadForest()
    {
        SceneManager.LoadScene("Game Scene"); //Add the scene name when scene is created
    }

    //Loads the Mansion/Castle game scene
    public void LoadCastle()
    {
        SceneManager.LoadScene("Castle Scene"); //Add the scene name when scene is created
    }

    //Loads the Village game scene
    public void LoadVillage()
    {
        SceneManager.LoadScene("Village Scene"); //Add the scene name when scene is created
    }

    //Loads the Win Game scene
    public void LoadWinScreen()
    {
        SceneManager.LoadScene("Win Screen");
    }

    //Loads the Lose Game scene
    public void LoadLoseScreen()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    //Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }

}