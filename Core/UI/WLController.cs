using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WLController : MonoBehaviour
{
    /* 
        NOTE This is probably not the best way to structure it so 
        it may need to be refactored in the future.
    */

    public string mainMenu; // NOTE for one thing, maybe centralize the name of this variable so that changing it will only be needed in ONE scene.
    public string nextLevel;
    public GameObject restart;
    public GameObject area;
    public GameObject textPanel;
    public static WLController instance;
    string message;
    bool hasWon = false;

    void Awake()
    {
        instance = this;
        area.SetActive(false);
    }

    void Start()
    {
        Game_Manager.instance.GameWon += onGameWon;
        Game_Manager.instance.GameLost += onGameLost;
    }

    private void onGameWon(object sender, EventArgs e)
    {
        displayWin();
    }

    private void onGameLost(object sender, EventArgs e)
    {
        displayLoss();
    }

    public void displayWin()
    {
        area.SetActive(true);
        message = "Victory";
        restart.GetComponent<Text>().text = "Next Level";
        hasWon = true;
        displayMessage();
    }

    public void displayLoss()
    {
        area.SetActive(true);
        message = "Defeat";
        hasWon = false;
        displayMessage();
    }

    private void displayMessage()
    {
        textPanel.GetComponent<TextMeshProUGUI>().text = message;
    }

    public void changeScene()
    {
        if (Time.timeScale == 0)
        { // FIXME Eventually, we have to go back and refactor that GODDAMN UI!
            Time.timeScale = 1;
        }
        AudioManager.instance.PlaySong(null);
        SceneManager.LoadScene(mainMenu);
    }

    public void reloadScene()
    {
        if (Time.timeScale == 0)
        { // FIXME Eventually, we have to go back and refactor that GODDAMN UI!
            Time.timeScale = 1;
        }
        AudioManager.instance.PlaySong(null);
        if (hasWon) LoadNextLevel();
        else RestartGame();
    }

    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

}