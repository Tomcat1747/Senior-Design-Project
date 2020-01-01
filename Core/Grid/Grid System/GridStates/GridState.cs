using System.Linq;
using UnityEngine;

public abstract class GridState {
    protected GridSystem _gridSystem;

    protected GridState(GridSystem gridSystem) {
        _gridSystem = gridSystem;
    }

    /// <summary>
    /// Method is called when a unit is clicked on.
    /// </summary>
    /// <param name="unit">Unit that was clicked.</param>
    public virtual void OnUnitClicked(Unit unit) {
    }

    /// <summary>
    /// Method is called when mouse exits cell's collider.
    /// </summary>
    /// <param name="cell">Cell that was deselected.</param>
    public virtual void OnCellDeselected(Cell cell) {
        cell.UnMark();
    }

    /// <summary>
    /// Method is called when mouse enters cell's collider.
    /// </summary>
    /// <param name="cell">Cell that was selected.</param>
    public virtual void OnCellSelected(Cell cell) {
        cell.MarkAsHighlighted();
    }

    /// <summary>
    /// Method is called when a cell is clicked.
    /// </summary>
    /// <param name="cell">Cell that was clicked.</param>
    public virtual void OnCellClicked(Cell cell) {
    }

    /// <summary>
    /// Method is called on transitioning into a state.
    /// </summary>
    public virtual void OnStateEnter() {
        if (_gridSystem.Units.Select(u => u.PlayerNumber).Distinct().ToList().Count == 1) {
           
            _gridSystem.GridState = new GridStateGameOver(_gridSystem);
        }
    }

    /// <summary>
    /// Method is called on transitioning out of a state.
    /// </summary>
    public virtual void OnStateExit() {
    }
}