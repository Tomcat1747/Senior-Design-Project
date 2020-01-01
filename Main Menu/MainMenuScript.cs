using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // This Function Will Allow the Player to Begin a New Game
    public void NewGame()
    {
        // Load the New Game Scene
        AudioManager.instance.PlaySong(null);
        SceneManager.LoadScene("1_Tutorial");
    }

    // This Function Will Allow the Player to Load a Game
    public void LoadGame()
    {
        // Load the Load Game Scene
        SceneManager.LoadScene("Save Menu");
    }

    // This Function Will Allow the Player to Quit Out of the Game
    public void QuitGame()
    {
        // Quit Out of the Game
        Application.Quit();
    }
}
