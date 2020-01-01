using System.Collections.Generic;
using UnityEngine;

public abstract class ICellGridBuilder : MonoBehaviour {
    public Transform CellsParent;
    public Transform DefaultParent;
    public abstract List<Cell> GenerateGrid();
}