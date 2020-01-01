using System;
using System.Collections.Generic;
using UnityEngine;
using UnitRendererLibrary;

namespace UnitDataLibrary
{
    // ENUMS
    public enum ClassType
    {
        Default, //0
        Pikeman, //1
        Rifleman, //2
        Cavalry, //3
        //Engineer, //4
        //Mage, //5
    }

    public enum HealthStatus
    {
        Alive, //0
        Injured, //1
        Gravely_Injured, //2
        Deceased, //3
    }

    public enum SpawnState
    {
        Active, //0
        Inactive, //1
    }

    /* 
        To solve the issue of moving huge chunks of data around, 
        the UnitData class has its variables broken down into 5 major classes
        This offers a great deal of modularity and ease of movement.
    */
    // All miscellaneous data is stored here
    [System.Serializable]
    public struct UnitID
    {
        public string FirstName;
        public string LastName;
        public string Nickname;
        [TextArea(15, 20)]
        public string bio;
        public Gender gender;
    }

    // All data relevant to unit stats is stored here
    [System.Serializable]
    public struct UnitStats
    {
        public int HP;
        public int STR;
        public int MAG;
        public int SPD;
        public int SKL;
        public int LCK;
        public int DEF;
        public int RES;

        public UnitStats(int[] data)
        {
            if (data.Length < 8)
            {
                Debug.LogError("Invalid Update:\n Input Size: " + data.Length + ",\n DataSize: 8");
            }
            HP = data[0];
            STR = data[1];
            MAG = data[2];
            SPD = data[3];
            SKL = data[4];
            LCK = data[5];
            DEF = data[6];
            RES = data[7];
        }

        public static implicit operator int[](UnitStats value)
        {
            return new int[] {
                value.HP,
                    value.STR,
                    value.MAG,
                    value.SPD,
                    value.SKL,
                    value.LCK,
                    value.DEF,
                    value.RES,
            };
        }
    }

    [System.Serializable]
    public class UnitInventory
    {
        public Weapon currentlyEquipped;
        public List<Item> unitInventory;
        //public Soldier attachedto;
        [SerializeField]
        private WeaponFactory Factory;


        public UnitInventory(int size = 5)
        {
            /*unitInventory = new List<Item>();
            currentlyEquipped = null;
            if (Factory != null)
            {
                unitInventory.Add(Factory.NewInstantiation());//Example of new weapon creation need to create new weapon prefab first t pass into weapon factory
            }*/

            //attachedto = null;
        }


        public void unequip()
        {
            if (currentlyEquipped != null)
            {
                currentlyEquipped.isHeldBy.unitData.Stats.STR -= currentlyEquipped.Mt;
                currentlyEquipped.isHeldBy.unitData.Stats.SKL -= currentlyEquipped.Crit;
                currentlyEquipped = null;
            }
        }

        public void equip(Weapon weapon)
        {
            if (unitInventory.Contains(weapon))
            {
                unequip();
                currentlyEquipped = weapon;
                weapon.isHeldBy.unitData.Stats.STR += weapon.Mt;
                weapon.isHeldBy.unitData.Stats.SKL += weapon.Crit;
            }
            else
            {
                Debug.Log("Does not belong to this soldier!"); //take out since this should not happen
            }
            //unit reference is handled in the item script.
        }

        public List<Item> Adding_Items(Item item)//Checks if bagsize has been met already
        {
            if (unitInventory.Count < 5)
            {
                unitInventory.Add(item);
            }
            //change the unit that this item is held by to the unit
            return unitInventory;
        }

        public List<Item> DeleteItem(Item item)
        {
            unitInventory.Remove(item);

            return unitInventory;
        }
        public void UseItem(Item item)
        {
            if (item.uses != 0)
            {
                item.Use(item.isHeldBy);
            }
        }//the usage of each item will be defined uniquely for every different item.

        //might move this to item manager since there's only one instance of the manager it will have access to all the inventories of each unit.
        /*public void Transfer(Unit unit, Item item)
        {
            if (unitInventory.Count < 5) //this should be if (unit.Inventory.Count < unit.bagsize)
            {
                unitInventory.Add(item);
                //this.DeleteItem(item);
            }
            else { return; }

            return;
        }*/
        // TODO function to equip item from unitInventory (takes name)
        // TODO function to unequip item from unitInventory () takes no arguments. 
        // TODO whatever functions you may need for the Unit to interface with your inventory.
    }

    // All data relevant to leveling up is stored here
    [System.Serializable]
    public struct UnitGrowthRates
    {
        public int g_HP;
        public int g_STR;
        public int g_MAG;
        public int g_SPD;
        public int g_SKL;
        public int g_LCK;
        public int g_DEF;
        public int g_RES;

        public UnitGrowthRates(int[] data)
        {
            if (data.Length < 8)
            {
                Debug.LogWarning("Invalid Update:\n Input Size: " + data.Length + ",\n DataSize: 8");
            }
            g_HP = data[0];
            g_STR = data[1];
            g_MAG = data[2];
            g_SPD = data[3];
            g_SKL = data[4];
            g_LCK = data[5];
            g_DEF = data[6];
            g_RES = data[7];
        }

        public static implicit operator int[](UnitGrowthRates value)
        {
            return new int[] {
                value.g_HP,
                    value.g_STR,
                    value.g_MAG,
                    value.g_SPD,
                    value.g_SKL,
                    value.g_LCK,
                    value.g_DEF,
                    value.g_RES,
            };
        }
    }

    [System.Serializable]
    public struct UnitCurrentStatus
    {
        public int currentHP;
        public int currentMOV;
        public int currentAP; // Action Points
        public int currentPNum; // Player Number
        public int currentTNum; // Team Numbers
    }

    [System.Serializable]
    public struct UnitPortrait
    {
        public Sprite male;
        public Sprite female;
        public GameObject g_male;
        public GameObject g_female;
    }
}

// Solves the serialization problem of Vector3's.
[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}