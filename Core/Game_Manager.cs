using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game_Manager : MonoBehaviour
{

    public event EventHandler GameWon;
    public event EventHandler GameLost;
    public static Game_Manager instance;
    public DialogueEvents dialogueEvents;
    public List<Objectives> GameWinConditions;
    public GameObject unitInfo;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GridSystem.instance.GameEnded += onGameEnded;
        DecideLoad();//if save file exists    
    }

    //TODO
    public void DecideLoad()
    {

        if (PlayerPrefs.GetInt("WithinLevel") == 1)
        {
            Load();
        }
        else DefaultLoad();
    }
    //TODO
    void DefaultLoad()
    {
        NovelController.instance.LoadChapterFile(dialogueEvents.folder + dialogueEvents.LevelStart);
    }
    ///Moved this loading functionality to the unit manager since objects should be responsible for themselves !!
    void Load()
    {
        Debug.Log("we made it in the load function inside game manager");
        foreach (UnitMarker m in UnitMarker.Markers) m.gameObject.SetActive(false);
        UnitMarker.Markers.Clear();
        SaveManager.instance.LoadLastScene(SaveManager.currentFile);

        PlayerPrefs.SetInt("WithinLevel", 0);
        PlayerPrefs.Save();

    }

    public void Save()
    {
        SaveManager.instance.Save(SaveManager.currentFile);//wos originally "grrrrr" name of test file
    }

    private void onGameEnded(object sender, EventArgs e)
    {
        // TODO Check objectives list here
        // Naive implementation: If HumanPlayer is the only player left, then win, else loss.
        GridSystem gridSystem = (GridSystem)sender;
        if (gridSystem.CurrentPlayer is HumanPlayer)
        {
            endGame(GameWon, dialogueEvents.LevelEndWin, dialogueEvents.folder);
        }
        else
        {
            endGame(GameLost, dialogueEvents.LevelEndLoss, dialogueEvents.folder);
        }
    }

    private void endGame(EventHandler x, string file, string folder = "")
    {
        if (file != "" || file != null)
        {
            GUIController.instance.StopAllCoroutines();
            if (unitInfo != null)
                unitInfo.SetActive(false); // FIXME Get rid of the GUIController so this patch doesn't exist.
            NovelController.instance.LoadChapterFile(folder + file);
        }
        else
        {
            if (x != null)
                x.Invoke(this, new EventArgs());
        }
    }

    [System.Serializable]
    public class Objectives
    {
        public string description;

        public int status;//TODO Make
    }

    [System.Serializable]
    public struct DialogueEvents
    {
        public string folder;
        /// <summary>Textfile that plays at the start of the game.</summary>
        public string LevelStart;

        // TODO implement mid-battle dialogue

        /// <summary>Textfile that plays at the end of the game if player wins.</summary>
        public string LevelEndWin;
        /// <summary>Textfile that plays at the end of the game if player loses.</summary>
        public string LevelEndLoss;
    }
}
