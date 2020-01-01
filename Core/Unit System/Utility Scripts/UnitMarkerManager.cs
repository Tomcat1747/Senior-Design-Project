using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMarkerManager : MonoBehaviour {

    public Transform gridSystem;

    void Start(){
        GridSystem.instance.GameStarted += onGameStarted;
    }

    /// <summary>
    /// Clear the unitMarker list if the game has started.
    /// </summary>
    void onGameStarted(object sender, EventArgs e){
        UnitMarker.Markers = null;
    }

    /// <summary>
    /// Snaps unit objects to the nearest cell.
    /// </summary>
    public void SnapToGrid () {
        Debug.Log ("Snapping unit to nearest tile.");
        List<Transform> cells = new List<Transform> ();

        foreach (Transform cell in gridSystem) {
            cells.Add (cell);
        }

        foreach (Transform unit in transform) {
            var closestCell = cells.OrderBy (h => Math.Abs ((h.transform.position - unit.transform.position).magnitude)).First ();
            if (!closestCell.GetComponent<Cell> ().IsTaken) {
                unit.position = closestCell.position;
                unit.position += new Vector3 (0, 0, -0.5f);
            } //Unit gets snapped to the nearest cell
        }
    }
}