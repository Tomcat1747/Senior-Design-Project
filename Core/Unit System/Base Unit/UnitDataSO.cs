using UnitDataLibrary;
using UnitRendererLibrary;
using UnityEngine;

[CreateAssetMenu (menuName = "Unit/Unit Data")]
public class UnitDataSO : ScriptableObject {
    public UnitID ID = new UnitID ();
    public ClassType ClassType;
    public UnitSpriteData SpriteData = new UnitSpriteData ();
    public UnitStats Stats = new UnitStats ();
    public UnitInventory Inventory = new UnitInventory ();
    public UnitGrowthRates GrowthRates = new UnitGrowthRates ();
}