using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//move all of this code into the soldier script once will is done.
public class InventoryofUnit : MonoBehaviour
{
    [SerializeField] public List<Item> Inventory;
    public int bagsize = 10;//maximum size of carryable items
    public List<Item> Adding_Items(Item item)//Checks if bagsize has been met already
    {
        if (Inventory.Count < bagsize) Inventory.Add(item);
        //item.isHeldBy = this; uncomment when moved to soldier
        return Inventory;
    }
    public bool CheckIfAvailable(Item item)
    {
        if (Inventory.Contains(item)) return true;
        else return false;
    }
    public List<Item> DeleteItem(Item item)
    {
        if (CheckIfAvailable(item)) Inventory.Remove(item);
        item.isHeldBy = null;
        return Inventory;
    }
    public void UseItem(Item item)
    {
        //item.Use(unit);       uncomment when moved to soldier class
    }//the usage of each item will be defined uniquely for every different item.
    public void Transfer(Unit unit, Item item)
    {
        if (Inventory.Count < bagsize) //this should be if (unit.Inventory.Count < unit.bagsize)
        {
            //unit.Inventory.Add(item);
            //this.DeleteItem(item);
        }
        else { return; }

        return;
    }




    //Talk to Will about possibly implementing the list of items(Inventory) as an enum to represent each slot i.e slot_1,slot_2...
    //This method should work fine, but I feel that the enum method will let the desigining of the Inventory UI be easier since every item slot will already be defined.
}
