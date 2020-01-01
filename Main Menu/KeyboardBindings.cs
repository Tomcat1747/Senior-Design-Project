using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardBindings : MonoBehaviour
{
    // Create a New Dictionary to Store the in Game Keybindings
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    // Create Text for Up, Down, Left, and Right to Represent their Respective Functions
    public Text up, down, left, right;
    // Create a Game Object Which Holds the Current Keys
    private GameObject currentKey;
    // Create a Color32 for the Default Color of the Remappable Buttons
    private Color32 defaultColor = new Color(199, 201, 197, 255);
    // Create a Color32 for the Selected Color of a Remapped Button
    private Color32 selectedColor = new Color(168, 234, 63, 255);
    // Is Called on the First Frame of the Game to Load the Default Keybindings
    void Start()
    {
        // Create the Default Keybinding for Up
        keyBindings.Add("Move Up", KeyCode.W);
        // Create the Default Keybinding for Down
        keyBindings.Add("Move Down", KeyCode.S);
        // Create the Default Keybinding for Left
        keyBindings.Add("Move Left", KeyCode.A);
        // Create the Default Keybinding for Right
        keyBindings.Add("Move Right", KeyCode.D);
        // Links Up to Move Up
        up.text = keyBindings["Move Up"].ToString();
        // Link Down to Move Down
        down.text = keyBindings["Move Down"].ToString();
        // Link Left to Move Left
        left.text = keyBindings["Move Left"].ToString();
        // Link Right to Move Right
        right.text = keyBindings["Move Right"].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the User Clicked a Button Connected to Move Up
        if (Input.GetKeyDown(keyBindings["Move Up"]))
        {
            // Display Up
            Debug.Log("Up");
        }
        // Check if the User Clicked a Button Connected to Move Down
        if (Input.GetKeyDown(keyBindings["Move Down"]))
        {
            // Display Up
            Debug.Log("Down");
        }
        // Check if the User Clicked a Button Connected to Move Left
        if (Input.GetKeyDown(keyBindings["Move Left"]))
        {
            // Display Up
            Debug.Log("Left");
        }
        // Check if the User Clicked a Button Connected to Move Right
        if (Input.GetKeyDown(keyBindings["Move Right"]))
        {
            // Display Up
            Debug.Log("Right");
        }
    }

    private void OnGUI()
    {
        // Check to Make Sure that the Current Key is Not Null
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keyBindings[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = defaultColor;
                currentKey = null;
            }
        }
    }

    // Allows the User to Change a Given Key
    public void alterKeys(GameObject selected)
    {
        // Check to See if the currentKey is Not Null
        if (currentKey != null)
        {
            // Change the Color of the Key's Button to defaultColor
            currentKey.GetComponent<Image>().color = defaultColor;
        }
        // Set the Current Key to be the One Selected
        currentKey = selected;
        // Change the Color of the Key's Button to selectedColor
        currentKey.GetComponent<Image>().color = selectedColor;
    }
}
