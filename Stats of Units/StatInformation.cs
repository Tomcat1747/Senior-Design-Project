using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatInformation : ScriptableObject
{
    public int LevelupMagic;
    public int LevelupDefense;
    public int LevelupAttack;
    public int LevelupSpeed;
    public int LevelupRes;

    public abstract void LevelUp(Unit unit);//Define unique levelup procedure for each unit
    
}

