using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public new string name;
    public Sprite picture = null;
    public Soldier isHeldBy = null;
    public int uses;
    public virtual void Use(Soldier soldier)
    {
        //Implement unique use case for each item
        Debug.Log("Item" + name + "was used");
    }


}
