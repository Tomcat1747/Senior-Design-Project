using System;
using System.Collections.Generic;
using System.Linq;
using RandomName;
using Saving;
using UnitDataLibrary;
using UnityEngine;
using UnitRendererLibrary;

public class UnitManager : MonoBehaviour, Saveable
{
    #region Singleton
    public static UnitManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Transform UnitPool;
    GridSystem gridSystem;
    ObjectPooler objectPooler;
    NameGenerator nameGenerator;
    void Start()
    {
        gridSystem = GridSystem.instance;
        gridSystem.LevelLoading += LoadingUnits;
        objectPooler = ObjectPooler.instance;
        nameGenerator = GetComponent<NameGenerator>();
        roster.AddRange(CustomRoster);
    }

    public ClassLibrarySO ClassLibrary;
    public List<UnitData> CustomRoster = new List<UnitData>();
    List<UnitData> roster = new List<UnitData>();

    // LINQ QUERIES
    /// <summary>
    /// Get all deceased units that belong to the requested player. Player 0 by default.
    /// </summary>
    public List<UnitData> getDeceasedUnits(int player = 0)
    {
        return roster.Where(unit => (unit.PlayerNumber == player && unit.HealthStatus == HealthStatus.Deceased)).ToList();
    }

    /// <summary>
    /// Get all living units that belong to the requested player. Player 0 by default.
    /// </summary>
    public List<UnitData> getLivingUnits(int player = 0)
    {
        return roster.Where(unit => (unit.PlayerNumber == player && unit.HealthStatus == HealthStatus.Alive)).ToList();
    }

    /// <summary>
    /// Get all units that are active in the given scene. Player 0 by default.
    /// </summary>
    public List<UnitData> getActiveUnits(int player)
    {
        return roster.Where(unit => (unit.PlayerNumber == player && unit.SpawnState == SpawnState.Active)).ToList();
    }

    // FUNCTIONS
    public void Initialize()
    {
        InitRoster();
    }

    protected void InitRoster()
    {

        //roster.AddRange(GetMarkers());//originally contained by the location of each of the markers. May not even be needed since taken care of inside the loadingunits function.
        // TODO establish path to savefile and initialize roster for the first time
    }

    protected List<UnitData> setroster(List<UnitData> savedRoster)
    {
        for (int x = 0; x < roster.Count; x = x + 1)
        {
            roster[x] = savedRoster[x];
        }
        return roster;
    }

    protected void CallRoster()
    {
        foreach (UnitData u in roster)
        {

        }
        // TODO call roster from savefile

    }

    protected virtual List<UnitData> GetMarkers()
    {
        List<UnitData> res = new List<UnitData>();
        foreach (UnitMarker m in UnitMarker.Markers)
        {
            UnitData unit;
            if (m.classType == ClassType.Default)
            {
                string[] defaultname = new string[] { "Training", "Dummy" };
                unit = new UnitData(m.PlayerNumber, m.classType, m.gameObject.transform.localPosition, defaultname);
            }
            else
            {
                Gender x = UnityEngine.Random.value > 0.5f ? Gender.Male : Gender.Female;
                string[] fullname = nameGenerator.getName(x == Gender.Male);
                unit = new UnitData(m.PlayerNumber, m.classType, m.gameObject.transform.localPosition, fullname);
                unit.ID.gender = x;
            }
            m.gameObject.SetActive(false);
            res.Add(unit);
        }
        return res;
    }

    /// <summary>
    /// Function called on event LevelLoading. Loads all units in the given roster.
    /// </summary>
    protected void LoadingUnits(object sender, EventArgs e)
    {
        roster.AddRange(GetMarkers());
        LoadUnits();
    }

    public void LoadUnits()
    {
        List<UnitData> activeUnits = new List<UnitData>();
        foreach (Player x in gridSystem.Players)
        {
            activeUnits.AddRange(getActiveUnits(x.PlayerNumber));
        }
        foreach (UnitData unitData in activeUnits)
        {
            GameObject x = ObjectPooler.instance.SpawnFromPool("Unit Pool", transform.position, Quaternion.identity);
            x.transform.SetParent(gameObject.transform);
            Soldier s = x.GetComponent<Soldier>();
            s.Initialize(unitData);
        }
    }

    // SAVE FUNCTIONS
    public object CaptureState()
    {

        List<UnitData> activeUnits = new List<UnitData>();
        foreach (Player x in gridSystem.Players)
        {
            activeUnits.AddRange(getActiveUnits(x.PlayerNumber));
        }
        for (int x = 0; x < activeUnits.Count; x = x + 1)
        {
            roster[x] = activeUnits[x];
        }

        foreach (UnitData r in roster) Debug.Log(r.ID.FirstName + r.ID.LastName);
        foreach (UnitData r in roster) Debug.Log(r.UnitPosition);
        return roster; //roster contains the locations of each of the unit markers not the actual positions of the unit data itself.
    }

    public void RestoreState(object state)
    {
        foreach (UnitMarker m in UnitMarker.Markers) m.gameObject.SetActive(false);
        UnitMarker.Markers.Clear();
        roster = ((List<UnitData>)state);
        foreach (UnitData r in roster) Debug.Log(r.ID.FirstName + r.ID.LastName);
        foreach (UnitData r in roster) Debug.Log(r.UnitPosition);
        GridSystem.instance.InitGame();
    }
}