using UnityEngine;

public class CameraDragMovement : MonoBehaviour
{
    public static CameraDragMovement Instance { get; private set; }

    public float dragSpeed = 2;
    public float zoomSpeed = 1;
    public float minZoom = 5;
    public float maxZoom = 20;

    public Vector2 minCameraPos;
    public Vector2 maxCameraPos;

    private Vector3 dragOrigin;

    public float focusSpeed = 2f;
    public float focusedZoom = 5f;
    private bool isFocusing = false;
    private Vector3 focusTarget;

    private bool isDraggingNPC = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        //DraggableNPC.OnNPCDragged += HandleNPCDragged;
    }

    void OnDisable()
    {
        //DraggableNPC.OnNPCDragged -= HandleNPCDragged;
    }

    private void HandleNPCDragged(bool isDragging)
    {
        isDraggingNPC = isDragging;
    }

    void Update()
    {
        if (isDraggingNPC)
        {
            return; // Skip camera movement logic if an NPC is being dragged
        }

        // Camera dragging
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragOrigin.z = 0;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            difference.z = 0;
            Camera.main.transform.position += difference;
            ConstrainCamera();
        }

        // Camera zooming
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);

        if (isFocusing)
        {
            // Smoothly move the camera towards the focus target
            transform.position = Vector3.Lerp(transform.position, focusTarget, Time.deltaTime * focusSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, focusedZoom, Time.deltaTime * focusSpeed);

            if (Vector3.Distance(transform.position, focusTarget) < 0.01f)
            {
                isFocusing = false; // Stop focusing once the target is reached
            }
        }
    }

    public void FocusOnRoom(Vector3 roomPosition)
    {
        focusTarget = new Vector3(roomPosition.x, roomPosition.y, transform.position.z);
        isFocusing = true;
    }

    private void ConstrainCamera()
    {
        var pos = Camera.main.transform.position;
        pos.x = Mathf.Clamp(pos.x, minCameraPos.x, maxCameraPos.x);
        pos.y = Mathf.Clamp(pos.y, minCameraPos.y, maxCameraPos.y);
        Camera.main.transform.position = pos;
    }
}