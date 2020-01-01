using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettingsScript : MonoBehaviour
{
    // Make a Reference to the Game's Audio Mixer
    public AudioMixer gameAudio;
    // Array of Possible Resolutions
    Resolution[] resolutions;
    // Create A Public Dropdown for the Resolutions
    public Dropdown gameResolutions;
    // Called at Game's Start
    void Start()
    {
        // Get the Computer's Screen Resolution
        resolutions = Screen.resolutions;
        // Remove the Default Options from the Dropdown Screen
        gameResolutions.ClearOptions();
        // Create a List of Strings to be Added to the Dropdown Screen
        List<string> resolutionOptions = new List<string>();
        // Set the Current Resolution Index to be Zero
        int currentResolution = 0;
        // Go Through the Array of Resolutions
        for(int i = 0; i < resolutions.Length; i++)
        {
            // Create a String for the Resolution
            string currentOption = resolutions[i].width + " x " + resolutions[i].height;
            // Add the Option to the List
            resolutionOptions.Add(currentOption);
            // Check if the Current Resolution is the Same as the Screen Resolution
            if ((resolutions[i].width == Screen.currentResolution.width) && (resolutions[i].height == Screen.currentResolution.height))
            {
                // Set the Current Resolution to be Equal to i
                currentResolution = i;
            }
        }
        // Add the Avalible Resolutions the the Dropdown Screen
        gameResolutions.AddOptions(resolutionOptions);
        // Set the Current Resolution
        gameResolutions.value = currentResolution;
        // Display the Current Resolution
        gameResolutions.RefreshShownValue();
    }
    // Allows the Player to Adjust the Volume of the Game
    public void AdjustVolume(float volume)
    {
        // Adjust the Game Volume
        gameAudio.SetFloat("volume", volume);
    }

    // Allows the User to Make the Game Fullscreen
    public void goFullscreen(bool isFullscreen)
    {
        // Set the Screen to Match the Boolean Expression 
        Screen.fullScreen = isFullscreen;
    }

    // Allows the Player to Set the Game Resolution
    public void setResolution(int resolution)
    {
        // Get the Selected Resolution
        Resolution currentResolution = resolutions[resolution];
        // Adjust the Screen Resolution
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
    }
}
