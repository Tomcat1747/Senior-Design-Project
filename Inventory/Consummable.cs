using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(menuName = "Consummable")]
public class Consummable : Item
{
    public new int uses = 5;
    public int hp_gain = 0;
    public int mp_gain = 0;
    public int defense_gain = 0;
    public override void Use(Soldier soldier)
    {
        base.Use(soldier);
        uses -= 1;
        soldier.HitPoints += hp_gain;
        soldier.unitData.Stats.DEF += defense_gain;
        soldier.unitData.Stats.MAG += mp_gain;//and so forth
        //discuss with Will on where the other attributes of the items are located.
    }


}
