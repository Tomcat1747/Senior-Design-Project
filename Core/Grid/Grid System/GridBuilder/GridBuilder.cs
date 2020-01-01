using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode()]
public class GridBuilder : ICellGridBuilder {
    ///<summary>
    ///Parent object to all Tilemap layers
    ///</summary>
    public Grid gridBase;
    /// <summary>
    /// Floor of the GridSystem
    /// </summary>
    public Tilemap floor;
    /// <summary>
    /// All other Tilemap layers above the floor
    /// </summary>
    public Tilemap[] terrainLayers;
    /// <summary>
    /// The tile scriptable object templates.
    /// </summary>
    public TileData[] tileTemplates;
    /// <summary>
    /// Default prefab for when a tile should NOT be navigated to. 
    /// </summary>
    public GameObject defaultPrefab;
    /// <summary>
    /// Aligned with terrainLayers to give custom properties
    /// </summary>
    public GameObject tilePrefab;

    public List<Cell> grid { get; private set;}

    public override List<Cell> GenerateGrid() {
        grid = new List<Cell>();
        Debug.Log("Generating Nodes");
        Filter();
        return grid;
    }

    private void Filter()
    {
        int idx = 0;
        int terrainKey = -1;
        TileBase tb;
        float newX;
        float newY;
        GameObject node;
        Tile tempTile;
        Cell tile;

        foreach (Vector3Int pos in floor.cellBounds.allPositionsWithin)
        {
            idx = 0;
            terrainKey = -1;
            foreach (Tilemap t in terrainLayers)
            {
                tb = t.GetTile(new Vector3Int(pos.x, pos.y, 0));
                if (tb != null)
                {
                    terrainKey = idx;
                }
                idx++;
            }

            newX = pos.x + 0.5f + gridBase.transform.position.x;
            newY = pos.y + 0.5f + gridBase.transform.position.y;
            
            if (terrainKey > -1)
            {
                node = Instantiate(tilePrefab, new Vector3(newX, newY, -0.5f), Quaternion.Euler(0, 0, 0));
                tempTile = node.GetComponent<Tile>();
                tempTile.tiledata = tileTemplates[terrainKey];
            }
            else
            {
                node = Instantiate(defaultPrefab, new Vector3(newX, newY, 0f), Quaternion.Euler(0, 0, 0));
            }

            tile = node.GetComponent<Cell>();
            
            if (tile != null)
            {
                tile.Coord = new Vector2(pos.x, pos.y);
                node.name = tile.name + " " + tile.Coord.x.ToString() + " : " + tile.Coord.y.ToString();
                node.transform.parent = CellsParent;
                grid.Add(tile);
            }
            else
            {
                node.transform.parent = DefaultParent;
            }
        }
    }

}