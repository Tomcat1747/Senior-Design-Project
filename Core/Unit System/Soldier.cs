using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit_2
{
    //[HideInInspector] // NOTE Uncomment when done debugging
    public UnitData unitData { get; protected set; }
    public event EventHandler UnitInit;

    // NOTE May want to consider a better way of implementing this
    public void checkAttackable()
    {
        // Checks if a unit is attackable so it does not auto wait.
        foreach (var currentUnit in GridSystem.instance.Units)
        {
            if (currentUnit.PlayerNumber.Equals(this.PlayerNumber))
                continue;
            if (this.IsUnitAttackable(currentUnit, this.Cell))
            {
                return;
            }
        }
        Action_Wait();
    }
    public void Action_Move()
    {
        base.OnMouseDown();
    }
    public void Action_Attack()
    {
        Debug.Log(name + " attacking!");
    }
    public void Action_Wait()
    {
        SetState(new UnitStateMarkedAsFinished(this));
        MovementPoints = 0;
        ActionPoints = 0;
    }

    // TODO: Update when finished integrating with UnitData
    public override void Initialize()
    {
        Skills = new List<Skill>();
        UnitState = new UnitStateNormal(this);
    }

    public void Initialize(UnitData data)
    {
        animator = GetComponent<Animator>();
        PlayerNumber = data.PlayerNumber;
        animator.runtimeAnimatorController = data.classData.AnimatorControllerList[PlayerNumber]; // TODO change this to dynamically change color
        gameObject.name = "[T" + PlayerNumber + "]: " + data.ID.FirstName + " " + data.ID.LastName;
        unitData = data;
        HitPoints = unitData.CurrentStatus.currentHP;
        MovementPoints = unitData.CurrentStatus.currentMOV;
        ActionPoints = unitData.CurrentStatus.currentAP;
        TotalHitPoints = unitData.Stats.HP;
        TotalMovementPoints = unitData.classData.MOV;
        TotalActionPoints = 1;

        gameObject.transform.localPosition = data.UnitPosition;
        equipped = new FakeWeapon(gameObject.name, unitData); // TODO Delete after integrating with Tahminur's Inventory System
        AttackRange = equipped.RNG;
        if (UnitInit != null)
            UnitInit.Invoke(this, new EventArgs());
    }

    public override List<Cell> GetAttackDestinations(List<Cell> cells, HashSet<Cell> _pathsInRange)
    {
        List<Cell> result = new List<Cell>();
        foreach (Cell cell in _pathsInRange)
        {
            result.AddRange(cells.FindAll(c => (
                !_pathsInRange.Contains(c) && c.GetDistance(cell) <= this.AttackRange
            )).Distinct());
        }
        return result;
    }

    /// <summary>
    /// Method deals damage to unit given as parameter.
    /// </summary>
    public override void DealDamage(Unit other)
    {
        if (isMoving)
            return;
        if (ActionPoints == 0)
            return;
        if (!IsUnitAttackable(other, Cell))
            return;
        // TODO Combat Forecast UI
        Soldier soldier = other as Soldier;
        Attack(soldier);
    }

    /// <summary>
    /// Method is called to allow the current unit to attack the other unit.
    /// </summary>
    protected virtual void Attack(Soldier target)
    {
        // Hit Chance
        if (AttemptHit(target as Soldier))
        {
            AttackFactor = AttemptCrit(target);
            base.DealDamage(target);
        }
        else
        {
            //print (name + " missed!"); // NOTE Remove when done debugging
            MarkAsAttacking(target);
            target.MarkAsAvoiding(this);
            ActionPoints--;
            if (ActionPoints == 0)
            {
                SetState(new UnitStateMarkedAsFinished(this));
                MovementPoints = 0;
            }
        }
        unitData.CurrentStatus.currentAP = ActionPoints;
    }

    public override void Move(Cell destinationCell, List<Cell> path)
    {
        base.Move(destinationCell, path);
        Vector3 pos = destinationCell.Coord;
        unitData.UnitPosition = pos + new Vector3(0f, 0f, -1f);
        unitData.CurrentStatus.currentMOV = MovementPoints;
        checkAttackable();
    }

    /// <summary>
    /// Attacking unit will call this Defend method on defending unit. 
    /// </summary>
    protected override void Defend(Unit defender, int damage)
    {
        DefenceFactor = GetDF(defender as Soldier);
        base.Defend(defender, damage);
        unitData.CurrentStatus.currentHP = HitPoints;
    }

    /// <summary>
    /// Method is called when units HP drops below 1.
    /// </summary>
    protected override void OnDestroyed()
    {
        //print (name + "has died!"); // NOTE Remove when done debugging
        Cell.IsTaken = false;
        MarkAsDestroyed();
        gameObject.transform.SetParent(UnitManager.instance.UnitPool);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Method is called to roll probability that this unit will hit "target" unit.
    /// </summary>
    protected virtual bool AttemptHit(Soldier target)
    {
        int battleHitRate = getHIT() - target.getAVO();
        int hitChance = rollChance(); // Random number from 0-100 (inclusive); rolled twice and averaged.
        if (hitChance < battleHitRate)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Method called to roll probability that this unit will perform a critical hit on "target" unit.
    /// </summary>
    protected virtual int AttemptCrit(Soldier target)
    {
        int critChance = rollChance();
        if (critChance < getCRIT())
        {
            MarkAsCritical(target);
            return GetAF() * 3;
        }
        else
        {
            return GetAF();
        }
    }

    /// <summary>
    /// Returns the average of two randomly generated numbers between min and max. By default, min and max are 0 and 101 [0-100].
    /// </summary>
    protected virtual int rollChance()
    {
        return (int)((UnityEngine.Random.value * 100) + (UnityEngine.Random.value * 100)) / 2;
    }

    #region Attack Data
    [SerializeField]
    FakeWeapon equipped; // TODO Delete after integrating with Tahminur's Inventory System

    // AF -> Weapon.MT + (MAG or STR) // Depends on what weapon is equipped
    public int GetAF()
    {
        int AF = equipped.weaponType == WeaponType.Physical ? unitData.Stats.STR : unitData.Stats.MAG;
        int res = AF + equipped.Mt;
        return res > 0 ? res : 0;
    }

    /// <summary>
    /// DF -> Weapon.MT + (RES or DEF): Depends on what weapon the attacker has equipped
    /// </summary>
    public int GetDF(Soldier attacker)
    {
        int res = attacker.equipped.weaponType == WeaponType.Physical ? unitData.Stats.DEF : unitData.Stats.RES;
        return res > 0 ? res : 0;
    }

    // HIT -> Weapon.HIT  + (SKL*3 + LCK)/2
    public int getHIT()
    {
        int res = equipped.Hit + (unitData.Stats.SKL * 3 + unitData.Stats.LCK) / 2;
        return roundResultToWithin(res);
    }

    // CRIT -> Weapon.CRIT + (SKL/2)
    public int getCRIT()
    {
        int res = equipped.Crit + (unitData.Stats.SKL / 2);
        return roundResultToWithin(res);
    }

    // AVO -> (SPD*3 + LCK)/2
    public int getAVO()
    {
        int res = (unitData.Stats.SPD * 3 + unitData.Stats.LCK) / 2;
        return roundResultToWithin(res);
    }

    /// <summary>
    /// Rounds result to min or max if result exceeds the range between min and max. Default set between 0 and 100.
    /// </summary>
    protected int roundResultToWithin(int res, int min = 0, int max = 100)
    {
        if (res < min)
        {
            return min;
        }
        else if (res > max)
        {
            return max;
        }
        return res;
    }
    #endregion

    // TODO Delete after integrating with Tahminur's Inventory System
    protected enum WeaponType
    {
        Physical, //0
        Magical, //1
    }

    // TODO Delete after integrating with Tahminur's Inventory System
    [System.Serializable]
    protected class FakeWeapon
    {
        public int Hit = 75;
        public int Crit = 15;
        public int Mt = 5;
        public int RNG = 1;
        public UnitData owner;
        public WeaponType weaponType;

        public FakeWeapon(string name, UnitData o)
        {
            this.owner = o;
            int r = UnityEngine.Random.Range(0, 2);
            weaponType = (WeaponType)r;
            if (o.classData.classType == UnitDataLibrary.ClassType.Rifleman)
            {
                RNG = 3;
            }
            else if (o.classData.classType == UnitDataLibrary.ClassType.Default)
            {
                RNG = 0;
            }
            else
            {
                RNG = 1;
            }
        }
    }

}