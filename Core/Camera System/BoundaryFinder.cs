using UnityEngine;
using UnityEngine.Tilemaps;

public class BoundaryFinder : MonoBehaviour
{
    protected PolygonCollider2D polygonCollider2D;
    protected Tilemap tilemap;

    void Awake()
    {
        if(gameObject.CompareTag("Tilemap Base") == false)
        {
            gameObject.tag = "Tilemap Base";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        tilemap = GetComponent<Tilemap>();
        if (polygonCollider2D == null)
        {
            polygonCollider2D = gameObject.AddComponent<PolygonCollider2D>();
        }
        if (tilemap == null)
        {
            Debug.LogError("Missing Tilemap Component.");
        }
        polygonCollider2D.isTrigger = true;
    }

    public void getBoundary()
    {
        Initialize();

        int xMin = tilemap.cellBounds.xMin;
        int xMax = tilemap.cellBounds.xMax;
        int yMin = tilemap.cellBounds.yMin;
        int yMax = tilemap.cellBounds.yMax;

        int[] newXPositions = { xMax, xMin, xMin, xMax };
        int[] newYPositions = { yMax, yMax, yMin, yMin };

        Vector2[] myPoints = new Vector2[4];

        for (int i = 0; i < 4; i++) // There should only ever be 4 points for the tilemap boundaries.
        {
            myPoints[i].x = newXPositions[i];
            myPoints[i].y = newYPositions[i];
            // print(myPoints[i]);
        }

        polygonCollider2D.points = myPoints;
    }

}