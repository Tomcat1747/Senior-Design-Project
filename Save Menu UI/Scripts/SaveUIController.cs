using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using Saving;
using System.Globalization;

public class SaveUIController : MonoBehaviour
{
    
    //stores the filepaths for each different file.
    public string[] filepaths;

    public void Start()
    {
        //obtains all save files from the directory
        filepaths = Directory.GetFiles(Application.persistentDataPath + "/SaveFiles","*.sav");
        //set's each file to a slot
        Set(filepaths);   
    }

    /// <summary>
    /// Find's the inactive save slots
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Set's each save slot in the UI with a save data if it exists.
    /// </summary>
    /// <param name="files"></param>
    public void Set(string[] files)
    {
        //retrieve saveslots
        GameObject SlotsWithData = GameObject.Find("SaveSlots");
        for (int x = 0; x < files.Length; x++)
        {
            //adds one since the slot numbers start from 1 and not 0
            x += 1;
            //we use this function below since the gameobject is inactive in the beginning
            GameObject saveslot = FindInActiveObjectByName("Save Slot " + x);
            //somehow have the save button clicked.
            saveslot.SetActive(true);
            x -= 1;
            //retrieves the name of file and returns it in .sav format
            string b = files[x].Split('/')[8];
            //gets the time a file was created
            string filetime = File.GetCreationTime(filepaths[x]).Date.ToString("MM-dd-yyyy");
            //Console.WriteLine(date1.ToString("d",
            //CultureInfo.CreateSpecificCulture("en-NZ")));
            //Gets rid of extension after period.
            string FileToBeNamed = b.Split('.')[0];
            // Activate Currently Saved
            saveslot.transform.Find("Saved Options").gameObject.SetActive(true);
            // Deactivate the New Save Options
            saveslot.transform.Find("New Save Options").gameObject.SetActive(false);
            // Set the Name of the File to be Displayed
            saveslot.transform.Find("Saved Options").transform.Find("File Name").GetComponent<TextMeshProUGUI>().text = FileToBeNamed;
            //changes the time
            saveslot.transform.Find("Saved Options").transform.Find("Time").GetComponent<TextMeshProUGUI>().text = filetime;
        }
    }

    /// <summary>
    /// Creates a file for data to be written to.
    /// </summary>
    public void createSaveFile()
    {
        // Get the Slot in Which the Save is Being Made
        GameObject saveSlot = GameObject.Find(EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name);
        // Get the Entered Filename
        string filename = saveSlot.transform.Find("New Save Options").transform.Find("Filename Input Field").transform.Find("Filename Area").transform.Find("Filename Text").GetComponent<TextMeshProUGUI>().text;
        // Check if the String that Contains the Filename is Empty
        if (filename.Length == 1)
        {
            // Make the Text that Asks the Player to Input the Filename Red
            saveSlot.transform.Find("New Save Options").transform.Find("Filename Input Field").transform.Find("Filename Area").transform.Find("Filename Placeholder").GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        // Otherwise
        else
        {
            // Set the Name of the File to be Displayed
            saveSlot.transform.Find("Saved Options").transform.Find("File Name").GetComponent<TextMeshProUGUI>().text = filename;
            // Set the Date/Time of the File to be Displayed
            saveSlot.transform.Find("Saved Options").transform.Find("Time").GetComponent<TextMeshProUGUI>().text = System.DateTime.Now.ToString();
            // Deactivate the New Save Options
            saveSlot.transform.Find("New Save Options").gameObject.SetActive(false);
            // Activate Currently Saved
            saveSlot.transform.Find("Saved Options").gameObject.SetActive(true);
            //gets the slot number to ensure that this slot is taken in the future.
            string slotnumber = saveSlot.transform.Find("New Save Options").transform.Find("Save Slot Number").GetComponent<TextMeshProUGUI>().text;
            //Creates the save file
            SaveManager.instance.Save(filename);
            //pass the name of the file to the savemanager to keep track of it for the following scenes.
            SaveManager.currentFile = filename;
            //Transition to first level.
            SceneManager.LoadScene("1_Tutorial");
            Debug.Log(SaveManager.currentFile);
        }

    }
    /// <summary>
    /// For changing filenames if that functionality is something you would like to implement in the future.
    /// </summary>
    /// <param name="newfilename"></param>
    public void ChangeFilename(string newfilename)
    {
        string filepath = Application.persistentDataPath + "/SaveFiles/" + "TempFile" + ".sav";
        string newfilepath = Application.persistentDataPath + "/SaveFiles/" + newfilename + ".sav";
        //change the name
        System.IO.File.Move(filepath,newfilepath);
    }
    /// <summary>
    /// Deletes the file from the menu
    /// </summary>
    public void deleteSaveFile()
    {
        // Get the Slot in Which the Save was Stored
        GameObject saveSlot = GameObject.Find(EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.name);
        //Get the filename to help display correct information on file delete menu
        string filename = saveSlot.transform.Find("Saved Options").transform.Find("File Name").GetComponent<TextMeshProUGUI>().text;
        // Display the Delete Menu to Display the Name of the File
        saveSlot.transform.Find("Delete Save Options").transform.Find("File Name Text").GetComponent<TextMeshProUGUI>().text = filename;
        //Deactivate the Saved Options since it is deleted.
        saveSlot.transform.Find("Saved Options").gameObject.SetActive(false);
        //Deactivate Currently Saved since savefile deleted
        saveSlot.transform.Find("Delete Save Options").gameObject.SetActive(false);
        //Activate the new save options again
        saveSlot.transform.Find("New Save Options").gameObject.SetActive(true);
        //deletes the save file
        deleteSave(filename);
        //resets the name to accept new input
        saveSlot.transform.Find("Saved Options").transform.Find("File Name").GetComponent<TextMeshProUGUI>().text = "";
        //resets it to the state it was in before.
        saveSlot.SetActive(false);
    }

    /// <summary>
    /// Deletes the file from the user's storage.
    /// </summary>
    /// <param name="FileName"></param>
    public void deleteSave(string FileName)
    {
        //retrieves the file
        string filepath = Application.persistentDataPath + "/SaveFiles/" + FileName + ".sav";
        //deletes the actual file
        File.Delete(filepath);
        
    }
    
    /// <summary>
    /// Loads the Game from the Seleted Save File only called if loading up from a previous save file
    /// </summary>
    public void loadGame()
    {
        // Get the Slot in Which the Save was Stored
        GameObject saveSlot = GameObject.Find(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        // Get the Name of the File
        string filename = saveSlot.transform.Find("Saved Options").transform.Find("File Name").GetComponent<TextMeshProUGUI>().text;
        //Passes the name to the SaveManager instance for it to handle future saves and loads.
        SaveManager.currentFile = filename;
        //Loads up the last scene it was in with all the appropriate data.
        SaveManager.instance.LoadLastScene(filename);
        Debug.Log(SaveManager.currentFile);
        saveSlot.SetActive(false);
    }
}
