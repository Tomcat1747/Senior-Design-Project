using System;
using UnityEngine;
using UnityEngine.UI;

public class TempGUIController : MonoBehaviour {
    public Button endTurnButton;
    private int turns;

    void Awake () {
        // Set the Current Number of Turns to One
        turns = 1;
    }

    void Start () {
        GridSystem.instance.TurnEnded += endTurn;
    }

    // Ends the Current Player Turn
    private void endTurn (object sender, EventArgs e) {
        turns++;
        // Sets Up the End Turn Button
        // endTurnButton.interactable = ((sender as GridSystem).CurrentPlayer is HumanPlayer);
    }

}