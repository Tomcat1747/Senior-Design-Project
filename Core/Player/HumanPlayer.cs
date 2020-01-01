/// <summary>
/// Class representing a human player.
/// </summary>
public class HumanPlayer : Player
{
    public override void Play(GridSystem cellGrid)
    {
        cellGrid.GridState = new GridStateWaitingForInput(cellGrid);
    }
}