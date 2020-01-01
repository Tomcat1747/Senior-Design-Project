using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public AudioSource audioSource; // Plays the Audio Clips
    public GridSystem gridSystem; // Grid System for the Level
    public GameObject terrainPanel; // Terrain & Counter Panel
    public GameObject mapPanel; // Map Panel
    public GameObject objectivePanel; // Objective Information Panel
    public GameObject unitInfoPanel; // Unit Information Panel
    public GameObject unitSkillPanel; // Unit Skill Panel
    public GameObject additionalInfoPanel; // Additional Information Panel
    public GameObject pauseMenu; // Pause Menu for the Game
    public GameObject damageCounter; // Display of Damage Dealt
    public GameObject actionButtons; // Actions that the Unit Can Perform
    public GameObject turnLabel; // Marks Whose Turn it is 
    public Canvas gameCanvas; // Canvas Being Used to Display the Relvant Information
    public AudioClip baseAttack; // Base Attack Sound Effect
    public AudioClip criticalAttack; // Critical Attack Sound Effect
    public AudioClip attckMiss; // Attack Miss Sound Effect
    public AudioClip back; // Back Sound Effect
    public AudioClip select; // Select Sound Effect
    private bool gameOver;
    private bool infoDisplayed;
    private int turns; // Count of Number of Turns
    private Button endTurnButton;
    private Button additionalInfoButton;
    private float itemNumber; // Number of the Current Item
    private bool additionalActive = false;
    private bool pauseMenuActive = false;
    private Soldier unit = null;
    private Vector2 startPoint;
    public static GUIController instance;
    // Initializes the Game Variables Before the Game Starts
    void Awake()
    {
        instance = this;
        startPoint = turnLabel.GetComponent<RectTransform>().anchoredPosition;
        //StartCoroutine(MoveTurnLabel());
        actionButtons.SetActive(false);
        gameOver = false;
        infoDisplayed = true;
        turns = 1;
        // Setup the End Turn Button
        endTurnButton = terrainPanel.transform.Find("End Turn Button").GetComponent<Button>();
        // Setup the Additional Information Button
        additionalInfoButton = unitInfoPanel.transform.Find("Additional Information Button").GetComponent<Button>();
        // Display the Turn Count to as One
        terrainPanel.transform.Find("Counter").GetComponent<TextMeshProUGUI>().text = turns.ToString();
        // Set the Unit Information Panel to be Inactive
        unitInfoPanel.SetActive(false);
        // Set the Unit Skill Panel to be Inactive
        unitSkillPanel.SetActive(false);

        gridSystem.GameStarted += gameStart;

        gridSystem.TurnEnded += endTurn;

        gridSystem.GameEnded += endGame;

        gridSystem.UnitAdded += onUnitAdded;
    }

    //Update Once Per Frame
    void Update()
    {
        // Have the Action Buttons Follow the Unit
        //ActionButtonsFollow (unit);
        // Check if the Player Clicked the Right Mouse Button
        if (Input.GetMouseButtonDown(1) && infoDisplayed)
        {
            //deselectUnit ();
            terrainPanel.SetActive(true);
            mapPanel.SetActive(true);
            objectivePanel.SetActive(true);
        }
        // Check if the Player Clicked the Escape Key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the Game is Paused
            if (pauseMenuActive)
            {
                // Resume the Game
                ExitPause();
            }
            // Otherwise
            else
            {
                // Pause the Game
                pauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && unit != null && !additionalActive)
        {
            //deselectUnit ();
        }
        // Set the Current Item Number to be Equal to the Current Value of the Slider
        itemNumber = additionalInfoPanel.transform.Find("Abilities").transform.Find("Item Slider").GetComponent<Slider>().value;
    }
    private void gameStart(object sender, EventArgs e)
    {
        mapPanel.SetActive(true);
        objectivePanel.SetActive(true);
        terrainPanel.SetActive(true);
        turnLabel.SetActive(true);
        // TODO
        TextMeshProUGUI tmp = objectivePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        foreach (Game_Manager.Objectives x in Game_Manager.instance.GameWinConditions)
        {
            tmp.text += " - " + x.description + "\n";
        }
        turnLabel.transform.Find("Turn Title").GetComponent<TextMeshProUGUI>().color = Color.white;
        turnLabel.transform.Find("Turn Title").GetComponent<TextMeshProUGUI>().text = "Player Turn";
        StartCoroutine(MoveTurnLabel());
    }

    // End the Current Player Turn
    private void endTurn(object sender, EventArgs e)
    {
        bool isPlayer = ((sender as GridSystem).CurrentPlayer is HumanPlayer);
        // Setup the End Turn Button
        endTurnButton.interactable = isPlayer;
        // Increment the Turn Count if it's the player's turn.
        turns = isPlayer ? turns += 1 : turns;
        // Update the Displayed Turn
        terrainPanel.transform.Find("Counter").GetComponent<TextMeshProUGUI>().text = turns.ToString();
        if (isPlayer)
        {
            turnLabel.transform.Find("Turn Title").GetComponent<TextMeshProUGUI>().color = Color.white;
            turnLabel.transform.Find("Turn Title").GetComponent<TextMeshProUGUI>().text = "Player Turn";
        }
        else
        {
            turnLabel.transform.Find("Turn Title").GetComponent<TextMeshProUGUI>().color = Color.red;
            turnLabel.transform.Find("Turn Title").GetComponent<TextMeshProUGUI>().text = "Enemy Turn";
        }
        StartCoroutine(MoveTurnLabel());
    }

    Coroutine _waitForKeyDown;
    // End the Game
    private void endGame(object sender, EventArgs e)
    {
        StopCoroutine(_waitForKeyDown);
        unitInfoPanel.SetActive(false);
        mapPanel.SetActive(false);
        objectivePanel.SetActive(false);
        terrainPanel.SetActive(false);
        turnLabel.SetActive(false);
        // Set Game Over to be True
        gameOver = true;
    }

    // Runs When a Unit is Attacked
    private void unitAttacked(object sender, AttackEventArgs e)
    {
        // Display the Damage Counter
        StartCoroutine(displayDamage(e.Damage, sender));
        // Check that the Current Player is Not the Human Player
        if (!(gridSystem.CurrentPlayer is HumanPlayer))
        {
            // Exit Out of the Function 
            return;
        }
        // Deselect that Unit
        deselectUnit();
        // Check if the Unit's Hit Points are Less Than or Equal to Zero
        if ((sender as Unit).HitPoints <= 0)
        {
            // Exit Out of the Function
            return;
        }
        // Select the Unit
    }

    // Runs When a Unit Becomes Deselected
    public void deselectUnit()
    {
        // Check if the Game is Over
        if (gameOver)
        {
            // Exit Out of the Function
            return;
        }
        // Set the Audio Clip to be Back
        audioSource.clip = back;
        // Play the Audio Clip
        StartCoroutine(PlaySound());
        // Set Unit to be NULL
        unit = null;
        // Deactivate the Action Buttons
        actionButtons.SetActive(false);
        // Deactivate the Unit Info Panel
        unitInfoPanel.SetActive(false);
        // Deactivate the Unit Skill Panel
        unitSkillPanel.SetActive(false);
        // Deactivate the Additional Information Panel
        additionalInfoPanel.SetActive(false);
        // Set the Information is Displayed to False
        infoDisplayed = false;
    }

    // Runs When a Unit is Selected
    private void selectUnit(object sender, EventArgs e)
    {
        // Debug.Log ("Select Unit"); // NOTE: Use this to keep track of unit selected
        // Check if the Game is Over the Additional Panel is Active
        if (gameOver || additionalActive)
        {
            // Exit Out of the Function
            return;
        }
        _waitForKeyDown = StartCoroutine(WaitForKeyDown(sender));
    }

    // Collects Information on the Unit
    private void setUnitInfo(object sender)
    {
        // Set a Variable to Hold the Current Unit
        var selectedUnit = sender as Soldier;
        /*
        actionButtons.transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => { Move (unit); });
        actionButtons.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (() => { Attack (unit); });
        actionButtons.transform.GetChild (2).GetComponent<Button> ().onClick.AddListener (() => { Wait (unit); });
        */
        // Get the Health Scale of the Unit
        float healthScale = ((float)selectedUnit.HitPoints / (float)selectedUnit.TotalHitPoints);
        // Set the Primary Health Bar
        unitInfoPanel.transform.Find("Health Bar").GetComponent<Image>().fillAmount = healthScale;
        // Set the Secondary Health Bar
        additionalInfoPanel.transform.Find("Health and Experience").transform.Find("Health Bar").GetComponent<Image>().fillAmount = healthScale;
        // Get the Experience Scale of the Unit
        // Needs to be Added
        // Get the Percentage of Health for the Unit
        int healthPercentage = (int)(healthScale * 100);
        // Set the Health Percentage
        unitInfoPanel.transform.Find("Health Percentage").GetComponent<TextMeshProUGUI>().text = healthPercentage.ToString() + "%";
        // Get the Unit's Name
        string unitName = getName(selectedUnit.name);
        // Set the Primary Name
        unitInfoPanel.transform.Find("Unit Name").GetComponent<TextMeshProUGUI>().text = unitName;
        // Set the Secondary Name
        additionalInfoPanel.transform.Find("Description Header").transform.Find("Unit Name").GetComponent<TextMeshProUGUI>().text = unitName;
        // Get the Rank of the Unit
        // Needs Rank to be Implemented
        // Get the Class of the Unit
        string unitClass = selectedUnit.unitData.ClassType.ToString();
        // Get the Level of the Unit
        string unitLevel = selectedUnit.unitData.Level.ToString();
        // Set the Primary Class
        unitInfoPanel.transform.Find("Unit Class").GetComponent<TextMeshProUGUI>().text = unitClass;
        // Set the Secondary Class and Level
        additionalInfoPanel.transform.Find("Description Header").transform.Find("Unit Class and Level").GetComponent<TextMeshProUGUI>().text = unitClass + " " + unitLevel;
        // Set a Variable to Hold the Currently Equipped Item
        string unitEquipment = "None";
        // Get the Equipped Weapon of the Use
        /*if(selectedUnit.unitData.Inventory.currentlyEquipped != null)
        {
            unitEquipment = selectedUnit.unitData.Inventory.currentlyEquipped.ToString();
        }*/
        // Set the Equipped Equipment
        unitInfoPanel.transform.Find("Unit Equipment").GetComponent<TextMeshProUGUI>().text = unitEquipment;
        // Get the Unit's Strength
        int unitSTR = selectedUnit.unitData.Stats.STR;
        // Set Strength
        additionalInfoPanel.transform.Find("Stats").transform.Find("Strength Amount").GetComponent<TextMeshProUGUI>().text = unitSTR.ToString();
        // Get the Unit's Magic
        int unitMAG = selectedUnit.unitData.Stats.MAG;
        // Set Magic
        additionalInfoPanel.transform.Find("Stats").transform.Find("Magic Amount").GetComponent<TextMeshProUGUI>().text = unitMAG.ToString();
        // Get the Unit's Skill
        int unitSKL = selectedUnit.unitData.Stats.SKL;
        // Set Skill
        additionalInfoPanel.transform.Find("Stats").transform.Find("Skill Amount").GetComponent<TextMeshProUGUI>().text = unitSKL.ToString();
        // Get the Unit's Speed
        int unitSPD = selectedUnit.unitData.Stats.SPD;
        // Set Speed
        additionalInfoPanel.transform.Find("Stats").transform.Find("Speed Amount").GetComponent<TextMeshProUGUI>().text = unitSPD.ToString();
        // Get the Unit's Luck
        int unitLUK = selectedUnit.unitData.Stats.LCK;
        // Set Luck
        additionalInfoPanel.transform.Find("Stats").transform.Find("Luck Amount").GetComponent<TextMeshProUGUI>().text = unitLUK.ToString();
        // Get the Unit's Defense
        int unitDEF = selectedUnit.unitData.Stats.DEF;
        // Set Defense
        additionalInfoPanel.transform.Find("Stats").transform.Find("Defense Amount").GetComponent<TextMeshProUGUI>().text = unitDEF.ToString();
        // Get the Unit's Resistance
        int unitRES = selectedUnit.unitData.Stats.RES;
        // Set Resistance
        additionalInfoPanel.transform.Find("Stats").transform.Find("Resistance Amount").GetComponent<TextMeshProUGUI>().text = unitRES.ToString();
        // Get the Attack Score of the Unti
        int unitATK = selectedUnit.GetAF();
        // Display the Attack Amount
        additionalInfoPanel.transform.Find("Combat Stats").transform.Find("Attack Amount").GetComponent<TextMeshProUGUI>().text = unitATK.ToString();
        // Get the Hit Chance of the Unit
        int unitHIT = selectedUnit.getHIT();
        // Display the Hit Amount
        additionalInfoPanel.transform.Find("Combat Stats").transform.Find("Hit Amount").GetComponent<TextMeshProUGUI>().text = unitHIT.ToString();
        // Get the Critical Chance of the Unit
        int unitCRT = selectedUnit.getCRIT();
        // Display the Critical Amount
        additionalInfoPanel.transform.Find("Combat Stats").transform.Find("Critical Amount").GetComponent<TextMeshProUGUI>().text = unitCRT.ToString();
        // Get the Advantage of the Unit
        int unitADV = selectedUnit.getAVO();
        // Display the Attack Amount
        additionalInfoPanel.transform.Find("Combat Stats").transform.Find("Advantage Amount").GetComponent<TextMeshProUGUI>().text = unitADV.ToString();
        // Get the List of Items that the Character Has
        //List<Item> unitItems = selectedUnit.unitData.Inventory.unitInventory;
        // Set the Value of the Slider to be Equal to Zero
        additionalInfoPanel.transform.Find("Abilities").transform.Find("Item Slider").GetComponent<Slider>().value = 0;
        // Set the Max Value of the Slider to be Equal to the Size of the Unit Item List
        // additionalInfoPanel.transform.Find("Abilities").transform.Find("Item Slider").GetComponent<Slider>().maxValue = unitItems.Count;
        // Check if the List of Items is Not Null

        // Get Image of Unit
        // FIXME WILL Fix gender here
        var u = selectedUnit.unitData.getFullBodyPortrait();
        var v = selectedUnit.unitData.getPanelPortrait();
        additionalInfoPanel.transform.Find("Unit Image").GetComponent<Image>().sprite = u;
        unitInfoPanel.transform.Find("Sprite Mask").transform.Find("Unit Image").gameObject.SetActive(true);
        unitInfoPanel.transform.Find("Sprite Mask").transform.Find("Unit Image").GetComponent<RectTransform>().localPosition = v.transform.GetComponent<RectTransform>().localPosition;
        unitInfoPanel.transform.Find("Sprite Mask").transform.Find("Unit Image").GetComponent<Image>().sprite = v.transform.GetComponent<Image>().sprite;
        unitInfoPanel.transform.Find("Sprite Mask").transform.Find("Unit Image").GetComponent<RectTransform>().localScale = new Vector2(2.5f, 2.5f);
    }

    // Runs When Terrian Tile is Deselected
    private void deselectTerrian(object sender, EventArgs e)
    {
        // Check if the Game is Over
        if (gameOver)
        {
            // Exit Out of the Function
            return;
        }
        // Set the Name of the Terrian to be Blank
        terrainPanel.transform.Find("Tile Name").GetComponent<TextMeshProUGUI>().text = "";
        // Set the Tile Information to be Blank
        terrainPanel.transform.Find("Tile Information").GetComponent<TextMeshProUGUI>().text = "";
    }

    // Runs When a Terrian Tile is Selected
    private void selectTerrain(object sender, EventArgs e)
    {
        // Check if the Game is Over
        if (gameOver)
        {
            // Exit Out of the Function
            return;
        }
        // Set a Variable to Hold the Current Tile
        var selectedTile = sender as Tile;
        // Set the Terrain Name
        terrainPanel.transform.Find("Tile Name").GetComponent<TextMeshProUGUI>().text = selectedTile.tiledata.name;
        // Set the Terrain Information
        terrainPanel.transform.Find("Tile Information").GetComponent<TextMeshProUGUI>().text = "Movement Cost: " + selectedTile.MovementCost.ToString();
    }

    // Registers Each Unit in the Game
    private void onUnitAdded(object sender, UnitCreatedEventArgs e)
    {
        // Register the Unit
        registerUnit(e.unit);
    }

    // Registers Unit in the Game
    private void registerUnit(Transform unit)
    {

        unit.GetComponent<Unit>().UnitHighlighted += selectUnit;

        unit.GetComponent<Unit>().UnitAttacked += unitAttacked;
    }

    // Displays the Additional Information Menu
    public void displayAdditional()
    {
        // Deactivate the Terrain Panel
        terrainPanel.SetActive(false);
        // Deactivate the Map Panel
        mapPanel.SetActive(false);
        // Deactivate the Objective Panel
        objectivePanel.SetActive(false);
        // Deactivate the Unit Info Panel
        unitInfoPanel.SetActive(false);
        // Deactivate the Unit Skill Panel
        unitSkillPanel.SetActive(false);
        // Activate the Additional Information Panel
        additionalInfoPanel.SetActive(true);
    }

    // Display the Regular Information
    public void displayInfo()
    {
        // Activate the Terrain Panel
        terrainPanel.SetActive(true);
        // Activate the Map Panel
        mapPanel.SetActive(true);
        // Activate the Objective Panel
        objectivePanel.SetActive(true);
        // Activate the Unit Info Panel
        unitInfoPanel.SetActive(true);
        // Activate the Unit Skill Panel
        //unitSkillPanel.SetActive(true);
        // Deactivate the Additional Information Panel
        additionalInfoPanel.SetActive(false);
    }

    private string getName(string name)
    {

        // Get Rid of the [T0]: or [T1]: in Front of the Name
        name = name.Substring(6, name.Length - 6);
        // Will Hold Just the Letters in the Name of the Character
        string placeholder = "";
        // Go Through the Characters in the Name
        foreach (char character in name)
        {
            // Check if the Character is a Letter
            if (char.IsLetter(character))
            {
                // Check if the Character is an Uppercase
                if (char.IsUpper(character))
                {
                    // Place a Space in Front of the Character
                    placeholder = placeholder + " " + character;
                }
                // Otherwise
                else
                {
                    // Append the Letter to the String
                    placeholder = placeholder + character;
                }
            }
        }
        // Set the Name to Become the Placeholder
        name = placeholder.Substring(1, placeholder.Length - 1);
        // Return the Name of the Character
        return name;
    }

    // Toggles On and Off the Additional Info Panel
    public void toggleAdditionalInfo()
    {
        // Check if the Additional Info Panel is Active
        if (additionalActive)
        {
            // Make it Inactive
            additionalActive = false;
        }
        // Otherwise
        else
        {
            // Make it Active
            additionalActive = true;
        }
    }

    // Pauses the Game
    private void pauseGame()
    {
        // Set the Game to be Paused
        pauseMenuActive = true;
        // Set the Pause Menu to be Displayed 
        pauseMenu.SetActive(true);
        // Set the Time Scale to be Zero
        Time.timeScale = 0;
    }

    // Displays the Damage Amount
    private IEnumerator displayDamage(int damage, object sender)
    {
        // Set a Variable to Hold the Current Unit
        var selectedUnit = sender as Soldier;
        // Get the X-Position of the Selected Unit
        float xPosition = selectedUnit.transform.position.x;
        // Get the Y-Position of the Selected Unit
        float yPosition = selectedUnit.transform.position.y;
        // Check if the Damage Amount is Zero
        if (damage == 0)
        {
            // Display the Attack as a Miss
            damageCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MISS";
            // Set the Audio Clip to be Attack Miss
            audioSource.clip = attckMiss;
        }
        // Othrwise
        else
        {
            // Set the Damage Amount to be Displayed
            damageCounter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();
            // Set the Audio Clip to be Base Attack
            audioSource.clip = baseAttack;
        }
        // Play the Audio Clip
        StartCoroutine(PlaySound());
        // Set the Position of the Damage Counter
        damageCounter.transform.position = new Vector2(xPosition, yPosition);
        // Set the Damage Counter to be Displayed
        damageCounter.SetActive(true);
        // Wait for One Seconds 
        yield return new WaitForSecondsRealtime(1);
        // Set the Damage Counter to be Hidden
        damageCounter.SetActive(false);
    }

    // Plays the Given Audio Effect
    IEnumerator PlaySound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
    }

    // Allows the Action Button to Follow the Character
    public void ActionButtonsFollow(object sender)
    {
        // Check to See if the Sender is NULL
        if (sender == null)
        {
            // Exit Out of the Function
            return;
        }
        // Set the Variable to Hold the Current Unit
        var selectedUnit = sender as Soldier;
        // Get the X-Position of the Selected Unit
        float xPosition = selectedUnit.transform.position.x;
        // Get the Y-Position of the Selected Unit
        float yPosition = selectedUnit.transform.position.y;
        actionButtons.transform.position = new Vector2(xPosition, yPosition);
    }

    // Lets Player Leave the Pause Menu
    public void ExitPause()
    {
        // Set Pause Menu Active to be False
        pauseMenuActive = false;
        // Set the Audio Clip to be Back
        audioSource.clip = back;
        // Play the Audio Clip
        StartCoroutine(PlaySound());
        // Deactivate the Pause Menu
        pauseMenu.SetActive(false);
        // Set Time Scale to be One
        Time.timeScale = 1;
    }

    // Lets Player Open Options Menu
    public void OpenOptions()
    {
        // Set the Audio Clip to be Select
        audioSource.clip = select;
        // Play the Audio Clip
        StartCoroutine(PlaySound());
        // Set the Pause Screen to be Inactivate
        pauseMenu.transform.GetChild(2).gameObject.SetActive(false);
        // Set the Options Screen to be Active
        pauseMenu.transform.GetChild(3).gameObject.SetActive(true);
    }

    // Lets Player Close Options Menu
    public void CloseOptions()
    {
        // Set the Audio Clip to be Back
        audioSource.clip = back;
        // Play the Audio Clip
        StartCoroutine(PlaySound());
        // Set the Pause Screen to be Active
        pauseMenu.transform.GetChild(2).gameObject.SetActive(true);
        // Set the Options Screen to be Inactive
        pauseMenu.transform.GetChild(3).gameObject.SetActive(false);
    }

    // Sends the Player Back to the Main Menu
    public void BackToMain()
    {
        // Set the Audio Clip to be Back
        audioSource.clip = back;
        // Play the Audio Clip
        StartCoroutine(PlaySound());
        // Set Time Scale to be One
        Time.timeScale = 1;
        // Load Back to the Main Menu
        SceneManager.LoadScene(0);
    }
    IEnumerator WaitForKeyDown(object sender)
    {
        bool pressed = false;
        while (!pressed)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pressed = true;
                // Check if the Information is Currently Displayed
                if (infoDisplayed)
                {
                    // Deselect the Unit
                    deselectUnit();
                }
                var selectedUnit = sender as Soldier;
                // Check if the Unit is Under the Player's Control
                if (selectedUnit.PlayerNumber == 0)
                {
                    // Set Unit to be the Sender
                    unit = selectedUnit;
                    // Set the Action Buttons to be Active
                    //actionButtons.SetActive (true);
                }
                // Otherwise
                else
                {
                    unit = selectedUnit;
                }
                // Set the Audio Clip to be Select
                audioSource.clip = select;
                // Play the Audio Clip
                StartCoroutine(PlaySound());
                // Get the Information About the Unit
                setUnitInfo(sender);
                // Display the Primary Information About the Unit
                unitInfoPanel.SetActive(true);
                // Display the Unit Skills of the Unit
                //unitSkillPanel.SetActive (true);
                // Set that Information is Displayed
                infoDisplayed = true;
                break;
            }
            yield return null; //you might want to only do this check once per frame -> yield return new WaitForEndOfFrame();
        }
    }

    public void Move(Soldier given)
    {
        if (given == null)
            return;
        // Move the Solider
        deselectUnit();
        given.Action_Move();
    }

    public void Attack(Soldier given)
    {
        if (given == null)
            return;
        // Have the Solider Attack
        deselectUnit();
        given.Action_Attack();
    }

    public void Wait(Soldier given)
    {
        if (given == null)
            return;
        // Have the Solider Wait
        deselectUnit();
        given.Action_Wait();
    }

    IEnumerator MoveTurnLabel()
    {
        turnLabel.GetComponent<RectTransform>().anchoredPosition = startPoint;
        // Get the Current Position of the Turn Label
        while (turnLabel.GetComponent<RectTransform>().anchoredPosition.y > (Math.Abs(startPoint.y)) * -1.5)
        {
            turnLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPoint.x, turnLabel.GetComponent<RectTransform>().anchoredPosition.y - (100 * Time.deltaTime));
            yield return null;
        }
        yield break;
    }
}