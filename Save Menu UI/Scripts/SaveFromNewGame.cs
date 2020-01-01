using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFromNewGame : MonoBehaviour
{
    //public string x;
    public string[] filenames;

    public void LoadSaveMenu()
    {
        if (Time.timeScale == 0)
        { // FIXME Eventually, we have to go back and refactor that GODDAMN UI!
            Time.timeScale = 1;
        }

        if (SaveManager.currentFile=="")
        {
            SaveManager.instance.Save("nameNotGiven");
        }
        else
        {
            SaveManager.instance.Save(SaveManager.currentFile);
        }
        
        AudioManager.instance.PlaySong(null);
        SceneManager.LoadScene("Save Menu");
             
    }
}
