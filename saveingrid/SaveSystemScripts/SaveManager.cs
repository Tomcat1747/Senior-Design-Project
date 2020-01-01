using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    /// <summary>
    /// Invoke when saving the game.
    /// </summary>
    public event EventHandler SavingGame;
    /// <summary>
    /// Invoke when loading the game.
    /// </summary>
    public event EventHandler LoadingGame;
    /// <summary>
    /// Static reference to the SaveManager in the given scene.
    /// </summary>
    public static SaveManager instance;

    public static string currentFile;

    protected virtual void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Loads up the approporiate scene
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>

    public void LoadLastScene(string filename) 
    {
        Dictionary<string, object> Data = LoadGameState(filename);

        
        if (Data.ContainsKey("LastSceneSavedIndex"))
        {
            int Scenenumber = (int)Data["LastSceneSavedIndex"];

            if (Scenenumber != SceneManager.GetActiveScene().buildIndex)
            {
                //Load up the scene
                SceneManager.LoadSceneAsync(Scenenumber);
            }
        }
        Load(filename);
    }


    /// <summary>
    /// Saves the game into a dictionary.
    /// </summary>
    /// <param name="filename"></param>
    public virtual void Save(string filename)
    {
        if (SavingGame != null)
        {
            SavingGame.Invoke(this, new EventArgs());
        }
        Dictionary<string, object> state = LoadGameState(filename);
        CaptureState(state);
        SaveGameState(filename, state);
    }
    
    /// <summary>
    /// Loads game from serialized dictionary.
    /// </summary>
    /// <param name="filename"></param>
    public virtual void Load(string filename)
    {
        
        RestoreState(LoadGameState(filename));
        if (LoadingGame != null)
        {
            LoadingGame.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// Saves all SaveEntity components to the dictionary called state
    /// </summary>
    /// <param name="state"></param>
    protected virtual void CaptureState(Dictionary<string, object> state)
    {
        foreach(SaveEntity saving in FindObjectsOfType<SaveEntity>())
        {
            state[saving.GetUniqueID()] = saving.CaptureState();
        }
        //add in a new state in order to track which scene was the last scene saved for future load capabilities.

        state["LastSceneSavedIndex"] = SceneManager.GetActiveScene().buildIndex;

    }

    /// <summary>
    /// For each SaveEntity, load the restore state
    /// </summary>
    /// <param name="state"></param>
    protected virtual void RestoreState(Dictionary<string, object> state)
    {
        foreach(SaveEntity entity in FindObjectsOfType<SaveEntity>())
        {
            string id = entity.GetUniqueID();
            if (state.ContainsKey(id))
            {
                entity.RestoreState(state[id]);
            }
        }
    }
    /// <summary>
    /// Performs the serialization and creation of the file that will be stored on user's system.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="state"></param>
    protected virtual void SaveGameState(string filename, object state)
    {
        string path = getSavePath(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, state);
        stream.Close();
    }

    protected virtual Dictionary<string, object> LoadGameState(string filename)
    {
        string path = getSavePath(filename);
        if (!File.Exists(path))
        {
            return new Dictionary<string, object>();
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        Dictionary<string, object> data = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();
        return data;
    }

    protected virtual string getSavePath(string filename)
    {
        string savePath = Application.persistentDataPath + "/SaveFiles";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        return savePath + "/" + filename + ".sav";
    }

}

