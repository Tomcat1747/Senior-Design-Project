using System.Collections;
using System.Collections.Generic;
using UnitDataLibrary;
using UnitRendererLibrary;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public UnitID ID = new UnitID();
    //public CustomSpriteData CustomSprite = new CustomSpriteData();
    public UnitStats Stats = new UnitStats();
    //public UnitInventory Inventory = new UnitInventory();
    public UnitGrowthRates GrowthRates = new UnitGrowthRates();
    public UnitCurrentStatus CurrentStatus = new UnitCurrentStatus();
    public SerializableVector3 UnitPosition = new SerializableVector3();//this is not serializable which conflicts with save talk to will about changing this.
    public int Level;
    const int LevelCap = 99;
    public int PlayerNumber;
    public int TeamNumber;

    // Enum States
    public SpawnState SpawnState;
    public HealthStatus HealthStatus;
    public ClassType ClassType;
    public ClassData classData { get { return UnitManager.instance.ClassLibrary.getClass(ClassType); } }

    // Constructors
    public UnitData(int p, Vector3 position, string[] name, int level = 1)
    {
        PlayerNumber = p;
        // TODO Randomly generate unit with random classType
        Initialize();
    }

    public UnitData(int p, Vector3 position, UnitDataSO u)
    {
        PlayerNumber = p;
        ID = u.ID;
        ClassType = u.ClassType;
        // TODO SpriteData
        Stats = u.Stats;
        // TODO Inventory
        GrowthRates = u.GrowthRates;
        Initialize();
    }

    public UnitData(int p, ClassType x, Vector3 position, string[] name, int level = 1, SpawnState state = SpawnState.Active)
    {
        ID.FirstName = name[0];
        ID.LastName = name[1];
        PlayerNumber = p;
        ClassType = x;
        UnitPosition = position;
        SpawnState = state;
        RandomizeStats();
        Initialize();
        // TODO Randomize growth rates too!
    }

    /// <summary>
    /// Only called ONCE. Synchronizes.
    /// </summary>
    protected void Initialize()
    {
        CurrentStatus.currentHP = Stats.HP;
        CurrentStatus.currentMOV = classData.MOV;
        CurrentStatus.currentAP = classData.ActionPoints;
        CurrentStatus.currentPNum = PlayerNumber;
        CurrentStatus.currentTNum = TeamNumber;
    }

    // Functions
    /// <summary>
    /// Randomly modifies unit stats anywhere between a min and max value. Default set to -2 to 2 (3).
    /// </summary>
    protected void RandomizeStats(int min = -2, int max = 3)
    {
        int[] stats = classData.BaseStats;
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] += Random.Range(min, max);
            if ((stats[i]) < 0)
            {
                stats[i] = 0;
            }
        }
        Stats = new UnitStats(stats);
    }

    public GameObject getPanelPortrait()
    {
        return ID.gender == Gender.Male ? classData.ClassPortrait.g_male : classData.ClassPortrait.g_female;
    }

    public Sprite getFullBodyPortrait()
    {
        return ID.gender == Gender.Male ? classData.ClassPortrait.male : classData.ClassPortrait.female;
    }

    protected void RandomizeGrowth()
    {
        // TODO Randomize Growth Rates
    }

    public void LevelUp(int target = 1)
    {
        if (target <= 0 || Level >= LevelCap) return;
        Level++;
        // TODO Grow the corresponding stats
        LevelUp(target - 1);
    }
}