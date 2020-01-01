using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using System;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 200f;

    protected CinemachineVirtualCamera myCamera;
    protected Tilemap mapBase;
    protected float maxZoomIn = 5f;
    protected float maxZoomOut = 6f;

    void Start()
    {
        Initialize();

        if (myCamera == null)
        {
            Debug.LogError("Missing Cinemachine Virtual Camera Component. Does the child object have this component?");
            return;
        }
        if (mapBase == null)
        {
            Debug.LogWarning("Unable to find Tilemap Base! Did you instantiate the Game Object?");
        }
        else
        {
            maxZoomOut = ((mapBase.cellBounds.xMax - mapBase.cellBounds.xMin) / 4) - 1;
            if (maxZoomOut < maxZoomIn)
            {
                Debug.LogWarning("maxZoomOut < maxZoomIn --> " + maxZoomOut + "<" + maxZoomIn);
                maxZoomOut = maxZoomIn + 1;
            }
        }
        NovelController.instance.chapterStarted += onChapterStarted;
        NovelController.instance.chapterFinished += onChapterFinished;
    }

    bool novelOperating = true;
    private void onChapterStarted(object sender, EventArgs e){
        novelOperating = true;
    }
    private void onChapterFinished(object sender, EventArgs e){
        novelOperating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!novelOperating){
            CameraMovement();
            CameraScroll();
        }
    }

    protected void Initialize()
    {
        GameObject BattleCamera = transform.GetChild(0).gameObject;
        GameObject Base = GameObject.FindGameObjectWithTag("Tilemap Base");
        if(Base == null)
        {
            Debug.LogError("Unable to find Base: " + Base.ToString() + ". Are you missing a tag?");
            return;
        }
        if (BattleCamera == null)
        {
            Debug.LogError("Unable to find child object Battle Camera: " + BattleCamera.ToString() + ". Is this object in the right place?");
            return;
        }


        myCamera = BattleCamera.GetComponent<CinemachineVirtualCamera>();
        CinemachineConfiner myConfiner = BattleCamera.GetComponent<CinemachineConfiner>();
        mapBase = Base.GetComponent<Tilemap>();
        myConfiner.m_BoundingShape2D = Base.GetComponent<PolygonCollider2D>();

    }

    protected void CameraScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float size = -1 * scroll * scrollSpeed * Time.deltaTime;
        float curr = myCamera.m_Lens.OrthographicSize + size;
        myCamera.m_Lens.OrthographicSize = Mathf.Clamp(curr, maxZoomIn, maxZoomOut);
    }

    protected void CameraMovement()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
