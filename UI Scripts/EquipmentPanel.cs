using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentPanel : MonoBehaviour
{
    // Drop Down for the Type of Item the Player Can Have
    public TMP_Dropdown itemOptions;
    // List of Weapons
    public GameObject weapons;
    // List of Potions
    public GameObject potions;
    // List of Other Items
    public GameObject other;

    // Update is called once per frame
    void Update()
    {
        // Get the Current Value of the Drop Down Box
        int currentValue = itemOptions.value;
        // Check if the Current Value is for the Weapons List
        if(currentValue == 0)
        {
            // Display the Weapons to the Player
            displayWeapons();
        }
        // Check if the Current Value is for the Potions List
        else if(currentValue == 1)
        {
            // Display the Potions to the Player
            displayPotions();
        }
        // Check if the Current Value is for the Other Items List
        else if(currentValue == 2)
        {
            // Display the Other Items to the Player
            displayOther();
        }
    }

    // Displays the List of Weapons
    private void displayWeapons()
    {
        // Set the Weapons Game Object to be Active
        weapons.SetActive(true);
        // Set the Potions Game Object to Not be Active
        potions.SetActive(false);
        // Set the Potions Game Object to Not be Actve
        other.SetActive(false);
    }

    // Displays the List of Potions
    private void displayPotions()
    {
        // Set the Weapons Game Object to Not be Active
        weapons.SetActive(false);
        // Set the Potions Game Object to be Active
        potions.SetActive(true);
        // Set the Potions Game Object to Not be Actve
        other.SetActive(false);
    }

    // Displays the List of Other Items
    private void displayOther()
    {
        // Set the Weapons Game Object to Not be Active
        weapons.SetActive(false);
        // Set the Potions Game Object to Not be Active
        potions.SetActive(false);
        // Set the Potions Game Object to be Actve
        other.SetActive(true);
    }
}
