class GridStateWaitingForInput : GridState
{
    public GridStateWaitingForInput(GridSystem gridSystem) : base(gridSystem)
    {
    }

    public override void OnUnitClicked(Unit unit)
    {
        if(unit.PlayerNumber.Equals(_gridSystem.CurrentPlayerNumber))
            _gridSystem.GridState = new GridStateUnitSelected(_gridSystem, unit); 
    }
}
