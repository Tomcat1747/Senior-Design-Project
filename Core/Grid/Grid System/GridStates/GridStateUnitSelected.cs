using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class GridStateUnitSelected : GridState
{
    private Unit _unit;
    private HashSet<Cell> _pathsInRange;
    private List<Cell> _pathsInAttackRange;
    private List<Unit> _unitsInRange;

    private Cell _unitCell;

    private List<Cell> _currentPath;

    public GridStateUnitSelected(GridSystem gridSystem, Unit unit) : base(gridSystem)
    {
        _unit = unit;
        _pathsInRange = new HashSet<Cell>();
        _currentPath = new List<Cell>();
        _unitsInRange = new List<Unit>();
    }

    public override void OnCellClicked(Cell cell)
    {
        if (_unit.isMoving)
            return;
        if (cell.IsTaken || !_pathsInRange.Contains(cell))
        {
            _gridSystem.GridState = new GridStateWaitingForInput(_gridSystem);
            return;
        }

        var path = _unit.FindPath(_gridSystem.Cells, cell);
        _unit.Move(cell, path);
        _gridSystem.GridState = new GridStateUnitSelected(_gridSystem, _unit);
    }
    public override void OnUnitClicked(Unit unit)
    {
        if (unit.Equals(_unit) || _unit.isMoving)
            return;

        if (_unitsInRange.Contains(unit) && _unit.ActionPoints > 0)
        {
            _unit.DealDamage(unit);
            _gridSystem.GridState = new GridStateUnitSelected(_gridSystem, _unit);
        }

        if (unit.PlayerNumber.Equals(_unit.PlayerNumber))
        {
            _gridSystem.GridState = new GridStateUnitSelected(_gridSystem, unit);
        }

    }
    public override void OnCellDeselected(Cell cell)
    {
        base.OnCellDeselected(cell);
        if (_pathsInAttackRange.Contains(cell))
        {
            cell.MarkAsAttackable();
        }
        foreach (var _cell in _currentPath)
        {
            if (_pathsInRange.Contains(_cell))
                _cell.MarkAsReachable();
            else
                _cell.UnMark();
        }
    }
    public override void OnCellSelected(Cell cell)
    {
        base.OnCellSelected(cell);
        if (_pathsInAttackRange.Contains(cell))
        {
            cell.MarkAsHighlightAttackable();
        }
        if (!_pathsInRange.Contains(cell)) return;

        _currentPath = _unit.FindPath(_gridSystem.Cells, cell);
        foreach (var _cell in _currentPath)
        {
            _cell.MarkAsPath();
        }
    }

    public override void OnStateEnter()
    {

        _unit.OnUnitSelected();
        _unitCell = _unit.Cell;

        _pathsInRange = _unit.GetAvailableDestinations(_gridSystem.Cells);
        _pathsInAttackRange = _unit.GetAttackDestinations(_gridSystem.Cells, _pathsInRange);
        var cellsNotInRange = _gridSystem.Cells.Except(_pathsInAttackRange);

        foreach (var cell in cellsNotInRange)
        {
            cell.UnMark();
        }
        foreach (var cell in _pathsInAttackRange)
        {
            cell.MarkAsAttackable();
        }
        foreach (var cell in _pathsInRange)
        {
            cell.MarkAsReachable();
        }

        if (_unit.ActionPoints <= 0) return;

        foreach (var currentUnit in _gridSystem.Units)
        {
            if (currentUnit.PlayerNumber.Equals(_unit.PlayerNumber))
            {
                currentUnit.Cell.MarkAsFriendly();
            }
            else if (_unit.IsUnitAttackable(currentUnit, _unit.Cell))
            {
                currentUnit.Cell.MarkAsPath();
                currentUnit.SetState(new UnitStateMarkedAsReachableEnemy(currentUnit));
                _unitsInRange.Add(currentUnit);
            }
        }

        _unit.Cell.MarkAsPath();

        if (_unitCell.Neighbors.FindAll(c => c.MovementCost <= _unit.MovementPoints).Count == 0
            && _unitsInRange.Count == 0)
            _unit.SetState(new UnitStateMarkedAsFinished(_unit));
    }
    public override void OnStateExit()
    {
        _unit.OnUnitDeselected();
        foreach (var unit in _unitsInRange)
        {
            if (unit == null) continue;
            unit.SetState(new UnitStateNormal(unit));
        }
        foreach (var cell in _gridSystem.Cells)
        {
            cell.UnMark();
        }
    }
}

