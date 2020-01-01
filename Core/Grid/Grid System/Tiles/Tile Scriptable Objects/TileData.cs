using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TileData")]

public class TileData : ScriptableObject
{
    public int MovementCost;

    public int Def_Bonus;

    public int AVO_Bonus;

    public int HP_Factor;

    public bool isObstacle;

}
