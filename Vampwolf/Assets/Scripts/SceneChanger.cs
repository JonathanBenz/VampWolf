using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //Loads the main hub/overworld/shop game scene
    public void LoadHub()
    {
        SceneManager.LoadScene("Game Scene");
    }

    // //Loads the Forest/Thicket game scene
    // public void LoadForest()
    // {
    //     SceneManager.LoadScene(); //Add the scene name when scene is created
    // }

    // //Loads the Mansion/Castle game scene
    // public void LoadCastle()
    // {
    //     SceneManager.LoadScene(); //Add the scene name when scene is created
    // }

    // //Loads the Village game scene
    // public void LoadVillage()
    // {
    //     SceneManager.LoadScene(); //Add the scene name when scene is created
    // }

    // //Loads the Win Game scene
    // public void LoadWinScreen()
    // {
    //     SceneManager.LoadScene(); //Add the scene name when scene is created
    // }

    // //Loads the Lose Game scene
    // public void LoadLoseScreen()
    // {
    //     SceneManager.LoadScene(); //Add the scene name when scene is created
    // }

    //Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }



}