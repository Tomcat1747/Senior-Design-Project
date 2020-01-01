using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//[CreateAssetMenu(menuName = "weapon")]
public class Weapon : Item
    {
    public int Hit = 0;
    public int Crit = 0;
    public int Mt = 0;
    //public enum WeaponType { Physical , Magical };
    public enum WeaponType { Physical};
    public override void Use(Soldier soldier)
    {
        base.Use(soldier);
        //This use case will handle average weapons if a weapon has a special skill it will be dealt with in the special_weapons.cs script.
    }

}
    