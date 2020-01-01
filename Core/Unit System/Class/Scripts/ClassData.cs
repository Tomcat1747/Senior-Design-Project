using System.Collections;
using System.Collections.Generic;
using UnitDataLibrary;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit/Class Data")]
public class ClassData : ScriptableObject
{

    public UnitStats BaseStats = new UnitStats();

    const int levelCap = 30;
    public int TotalActionPoints = 1;
    public int ActionPoints = 1;
    public int MOV = 5;

    public ClassType classType;
    public List<Skill> ActiveSkills;
    public List<Skill> AllSkills;

    public List<Sprite> classSprites;
    public List<RuntimeAnimatorController> AnimatorControllerList;

    public UnitPortrait ClassPortrait = new UnitPortrait();
}