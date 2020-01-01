using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : Square
{
    /// <summary>
    /// Used for Loading Unit positions
    /// </summary>
    public static Dictionary<Vector2, Cell> TilesOnGrid;
    public TileData tiledata;
    public int Def_Bonus;
    public int AVO_Bonus;
    public int HP_Factor;
    public bool isObstacle; // TODO will use this for flying units
    public SpriteRenderer frame;

    public override Vector3 GetCellDimensions()
    {
        var highlighter = transform.Find("Highlighter");
        var ret = highlighter.GetComponent<SpriteRenderer>().bounds.size;
        return ret * 0.98f;
    }

    public override void MarkAsReachable()
    {
        SetColor(new Color(0, 0, 1, 0.5f));
    }
    public override void MarkAsAttackable()
    {
        SetColor(new Color(1, 0, 0, 0.5f));
    }
    public override void MarkAsPath()
    {
        SetColor(new Color(1, 1, 0, 1));
    }
    public override void MarkAsFriendly()
    {
        SetColor(new Color(0, 1, 0, 1));
    }
    public override void MarkAsHighlighted()
    {
        SetColor(new Color(0.8f, 0.8f, 0.8f, 0.5f));
    }
    public override void MarkAsHighlightAttackable()
    {
        SetColor(new Color(1, 0, 0, 0.8f));
    }
    public override void UnMark()
    {
        SetColor(new Color(1, 1, 1, 0));
    }

    private void SetColor(Color color)
    {
        var highlighter = transform.Find("Highlighter");
        var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    private void Awake()
    {
        if (TilesOnGrid == null)
        {
            TilesOnGrid = new Dictionary<Vector2, Cell>();
        }
        TilesOnGrid[Coord] = this;
    }

    public void Start()
    {
        if (tiledata == null)
        {
            Debug.LogError("Missing tiledata");
        }
        MovementCost = tiledata.MovementCost;
        Def_Bonus = tiledata.Def_Bonus;
        AVO_Bonus = tiledata.AVO_Bonus;
        HP_Factor = tiledata.HP_Factor;
        isObstacle = tiledata.isObstacle;
    }

}