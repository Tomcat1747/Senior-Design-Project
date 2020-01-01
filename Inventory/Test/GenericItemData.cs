using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItemData : MonoBehaviour
{
    public new string name;
    public Sprite picture = null;
    public Soldier isHeldBy = null;
    public int uses;
    
    public void CreateItem()
    {
        
    }
}
