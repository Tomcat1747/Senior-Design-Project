using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Manager : MonoBehaviour
{
    public static Item_Manager instance;
    public List<Item> All_Poss_Items = new List<Item>();

    void Awake()
    {
        instance = this;    
    }

    Dictionary<Item,Soldier> All_Items;//for quick look up of what item belongs to who,
    
    void Start()
    {
        All_Items = new Dictionary<Item, Soldier>();

    }
}
