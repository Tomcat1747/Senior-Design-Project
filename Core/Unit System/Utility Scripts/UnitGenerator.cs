using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitGenerator : MonoBehaviour, IUnitGenerator {
    public Transform UnitsParent;
    public Transform CellsParent;

    /// <summary>
    /// Returns units that are already children of UnitsParent object.
    /// </summary>
    public List<Unit> SpawnUnits (List<Cell> cells) { // TODO Modify to call UnitManager to load list of units
        List<Unit> ret = new List<Unit> ();
        for (int i = 0; i < UnitsParent.childCount; i++) {
            var unit = UnitsParent.GetChild (i).GetComponent<Unit> ();
            if (unit != null) {
                var cell = cells.OrderBy (h => Math.Abs ((h.transform.position - unit.transform.position).magnitude)).First ();
                if (!cell.IsTaken) {
                    cell.IsTaken = true;
                    unit.Cell = cell;
                    unit.transform.position = cell.transform.position + new Vector3 (0, 0, -0.5f);
                    unit.Initialize ();
                    ret.Add (unit);
                } //Unit gets snapped to the nearest cell
                else {
                    Debug.Log ("Unable to find nearest cell. Destroying " + unit.name + ".");
                    gameObject.transform.SetParent (UnitManager.instance.UnitPool);
                    gameObject.SetActive (false);
                } //If the nearest cell is taken, the unit gets destroyed.
            }
        }
        return ret;
    }

    /// <summary>
    /// Snaps unit objects to the nearest cell.
    /// </summary>
    public void SnapToGrid () {
        Debug.Log ("Snapping unit to nearest tile.");
        List<Transform> cells = new List<Transform> ();

        foreach (Transform cell in CellsParent) {
            cells.Add (cell);
        }

        foreach (Transform unit in UnitsParent) {
            var closestCell = cells.OrderBy (h => Math.Abs ((h.transform.position - unit.transform.position).magnitude)).First ();
            if (!closestCell.GetComponent<Cell> ().IsTaken) {
                unit.position = closestCell.position;
                unit.position += new Vector3 (0, 0, -0.5f);
            } //Unit gets snapped to the nearest cell
        }
    }
}