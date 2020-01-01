using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GridSystem : MonoBehaviour
{
    /// <summary>
    /// Invoke before Initialize()
    /// </summary>
    public event EventHandler LevelLoading;
    /// <summary>
    /// Invoke after Initialize()
    /// </summary>
    public event EventHandler LevelLoadingDone;
    /// <summary>
    /// Invoke at the beginning of the game
    /// </summary>
    public event EventHandler GameStarted;
    /// <summary>
    /// Invoke when either the win condition is met
    /// </summary>
    public event EventHandler GameEnded;
    /// <summary>
    /// Invoke at the end of each turn
    /// </summary>
    public event EventHandler TurnEnded;
    /// <summary>
    /// Invoke on AddUnit()
    /// </summary>
    public event EventHandler<UnitCreatedEventArgs> UnitAdded;

    private GridState _gridState;
    public GridState GridState
    {
        private get { return _gridState; }
        set
        {
            if (_gridState != null) { _gridState.OnStateExit(); }
            _gridState = value;
            _gridState.OnStateEnter();
        }
    }
    public int NumberOfPlayers { get; private set; }

    public Player CurrentPlayer
    {
        get { return Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)); }
    }
    public int CurrentPlayerNumber { get; private set; }

    /// <summary>
    /// GameObject that holds player objects.
    /// </summary>
    public Transform PlayersParent;

    public List<Player> Players { get; private set; }
    public List<Cell> Cells { get; private set; }
    public List<Unit> Units { get; private set; }

    #region Singleton
    public static GridSystem instance;
    void Awake()
    {
        InitPlayers();
        instance = this;
    }
    #endregion

    bool start = false;
    void Update()
    {
        if (start && Input.GetKeyDown(KeyCode.E))
        {
            EndTurn();
        }
    }

    /// <summary>
    /// Call this to setup and start the game. Should be called after all units and players are set.
    /// </summary>
    public void InitGame()
    {
        if (LevelLoading != null)
            LevelLoading.Invoke(this, new EventArgs());

        Initialize();

        if (LevelLoadingDone != null)
            LevelLoadingDone.Invoke(this, new EventArgs());

        StartGame();
    }

    private void InitPlayers()
    {
        Players = new List<Player>();
        for (int i = 0; i < PlayersParent.childCount; i++)
        {
            var player = PlayersParent.GetChild(i).GetComponent<Player>();
            if (player != null)
                Players.Add(player);
            else
                Debug.LogError("Invalid object in Players Parent game object");
        }
        NumberOfPlayers = Players.Count;
        CurrentPlayerNumber = Players.Min(p => p.PlayerNumber);
    }

    private void Initialize()
    {
        Cells = new List<Cell>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i).gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                Cells.Add(cell);

            }

            else
                Debug.LogError("Invalid object in cells parent game object");
        }
        foreach (var cell in Cells)
        {
            cell.CellClicked += OnCellClicked;
            cell.CellHighlighted += OnCellHighlighted;
            cell.CellDehighlighted += OnCellDehighlighted;
            cell.Neighbors = cell.GetComponent<Cell>().GetNeighbours(Cells);
        }
        var unitGenerator = GetComponent<IUnitGenerator>();
        if (unitGenerator != null)
        {
            Units = unitGenerator.SpawnUnits(Cells);
            foreach (var unit in Units)
            {
                AddUnit(unit.GetComponent<Transform>());
            }
        }
        else
        {
            Debug.LogError("No IUnitGenerator script attached to cell grid");
        }
    }

    private void OnCellDehighlighted(object sender, EventArgs e)
    {
        GridState.OnCellDeselected(sender as Cell);
    }
    private void OnCellHighlighted(object sender, EventArgs e)
    {
        GridState.OnCellSelected(sender as Cell);
    }
    private void OnCellClicked(object sender, EventArgs e)
    {
        GridState.OnCellClicked(sender as Cell);
    }

    private void OnUnitClicked(object sender, EventArgs e)
    {
        GridState.OnUnitClicked(sender as Unit);
    }
    private void OnUnitDestroyed(object sender, AttackEventArgs e)
    {
        Units.Remove(sender as Unit);
        var totalPlayersAlive = Units.Select(u => u.PlayerNumber).Distinct().ToList(); //Checking if the game is over
        if (totalPlayersAlive.Count == 1)
        { // TODO Implement flexible win condition
            if (GameEnded != null)
            {
                GameEnded.Invoke(this, new EventArgs());

            }
        }
    }

    /// <summary>
    /// Adds unit to the game
    /// </summary>
    /// <param name="unit">Unit to add</param>
    public void AddUnit(Transform unit)
    {
        unit.GetComponent<Unit>().UnitClicked += OnUnitClicked;
        unit.GetComponent<Unit>().UnitDestroyed += OnUnitDestroyed;

        if (UnitAdded != null)
            UnitAdded.Invoke(this, new UnitCreatedEventArgs(unit));
    }

    /// <summary>
    /// Method is called once, at the beginning of the game.
    /// </summary>
    protected void StartGame()
    {
        if (GameStarted != null)
        {
            start = true; // FIXME centralize to a controls script later
            GameStarted.Invoke(this, new EventArgs());

        }
        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);
    }
    /// <summary>
    /// Method makes turn transitions. It is called by player at the end of his turn.
    /// </summary>
    public void EndTurn()
    {
        if (Units.Select(u => u.PlayerNumber).Distinct().Count() == 1)
        {
            return;
        }
        GridState = new GridStateTurnChanging(this);

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnEnd(); });

        CurrentPlayerNumber = (CurrentPlayerNumber + 1) % NumberOfPlayers;
        while (Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).Count == 0)
        {
            CurrentPlayerNumber = (CurrentPlayerNumber + 1) % NumberOfPlayers;
        }//Skipping players that are defeated.

        if (TurnEnded != null)
            TurnEnded.Invoke(this, new EventArgs());

        Units.FindAll(u => u.PlayerNumber.Equals(CurrentPlayerNumber)).ForEach(u => { u.OnTurnStart(); });
        Players.Find(p => p.PlayerNumber.Equals(CurrentPlayerNumber)).Play(this);
    }

    public void ResetGrid()
    {
        foreach (Cell c in Cells)
        {
            c.IsTaken = false;
        }
        Units = GetComponent<UnitGenerator>().SpawnUnits(Cells);
    }

}


