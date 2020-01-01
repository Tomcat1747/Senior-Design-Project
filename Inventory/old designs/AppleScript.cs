using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//example item
//[CreateAssetMenu (menuName = "apple")]
public class AppleScript : Item
{
    int hp_gain = 5;
    //public string nameofitem { get { return nameofitem; } set { nameofitem = value; } }

    public override void Use(Soldier soldier)
    {
        soldier.HitPoints += hp_gain;
    }
}
