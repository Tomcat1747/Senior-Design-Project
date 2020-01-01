using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//call to generate new weapons
public class WeaponFactory : GenericFactory<Weapon>
{
    

    /*public override Weapon NewInstantiation()
    {
        
        Weapon x = base.NewInstantiation();
        //add to item manager once instantiated, all items are moved from main inventory into each individual soldiers bag
        return x;
    }
    [SerializeField]
    public void createWeapon()
    {
        Item_Manager.instance.All_Poss_Items.Add(NewInstantiation());
    }*/
}
