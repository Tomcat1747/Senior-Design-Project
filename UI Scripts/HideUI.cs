using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HideUI : MonoBehaviour
{
    // Is the UI Showing
    private bool uiShowing = true;
    // Character Information Panel
    public GameObject characterPanel;
    // Skills Panel
    public GameObject skillsPanel;
    // Equipment Panel
    public GameObject equipmentPanel;
    // Objective Panel
    public GameObject objectivePanel;
    // Terrain Panel
    public GameObject terrainPanel;
    // All the Display Backgrounds
    public GameObject uiBackgrounds;

    // Toggles the UI
    public void toggleUI()
    {
        // Check if the UI is Currently Showing
        if(uiShowing)
        {
            // Turn Off All the Backgrounds of the Panels
            uiBackgrounds.SetActive(false);
            // Turn Off the Character Information Panel
            characterPanel.SetActive(false);
            // Turn Off the Skills Panel
            skillsPanel.SetActive(false);
            // Turn Off the Equipment Panel
            equipmentPanel.SetActive(false);
            // Turn Off the Objective Panel
            objectivePanel.SetActive(false);
            // Turn Off the Terrain Panel
            terrainPanel.SetActive(false);
            // Mark the the UI is Not Showing
            uiShowing = false;
        }
        // Otherwise
        else
        {
            // Turn On All the Backgrounds of the Panels
            uiBackgrounds.SetActive(true);
            // Turn On the Character Information Panel
            characterPanel.SetActive(true);
            // Turn On the Skills Panel
            skillsPanel.SetActive(true);
            // Turn On the Equipment Panel
            equipmentPanel.SetActive(true);
            // Turn On the Objective Panel
            objectivePanel.SetActive(true);
            // Turn On the Terrain Panel
            terrainPanel.SetActive(true);
            // Mark the the UI is Showing
            uiShowing = true;
        }
    }
}
