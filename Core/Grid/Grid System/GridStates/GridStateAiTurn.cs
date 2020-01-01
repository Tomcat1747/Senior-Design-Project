public class GridStateAiTurn : GridState
{
    public GridStateAiTurn(GridSystem gridSystem) : base(gridSystem)
    {      
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        foreach (var cell in _gridSystem.Cells)
        {
            cell.UnMark();
        }
    }
}