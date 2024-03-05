using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableNPC : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LayerMask roomLayerMask; // Assign this in the Inspector
    private Vector3 startPosition;
    private Camera mainCamera;
    private NPC npcComponent; // Reference to the NPC component
    private Transform originalParent;

    private void Awake()
    {
        mainCamera = Camera.main; // Cache the main camera for performance
        npcComponent = GetComponent<NPC>(); // Get the NPC component
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("NPC clicked");
        startPosition = transform.position; // Save starting position in case of invalid drop
        npcComponent.StopPacing(); // Stop pacing coroutine

        Room currentRoom = GetComponentInParent<Room>();
        if (currentRoom != null)
        {
            currentRoom.ClearNPC(); // Clear the NPC reference in the current room
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("NPC being dragged");
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = mainCamera.nearClipPlane; // Set appropriate distance from the camera
        transform.position = mainCamera.ScreenToWorldPoint(screenPoint);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 npcPosition = transform.position;
        npcPosition.z = 0; // Ensure Z-position is aligned with 2D gameplay plane

        Collider2D hitCollider = Physics2D.OverlapPoint(npcPosition, roomLayerMask);
        if (hitCollider != null && hitCollider.CompareTag("Room"))
        {
            PlaceNPCInRoom(hitCollider.gameObject);
            Debug.Log("NPC in room");

        }
        else
        {
            transform.position = startPosition; // Return to start position if drop is invalid
            Debug.Log("NPC not in room");
        }
    }

    private void PlaceNPCInRoom(GameObject roomGameObject)
    {
        Room room = roomGameObject.GetComponent<Room>();

        if (room != null && !room.IsOccupied())
        {
            Bounds roomBounds = room.GetComponent<Collider2D>().bounds;
            float npcHeight = GetComponent<Collider2D>().bounds.size.y;

            // Set the NPC position
            transform.position = new Vector3(transform.position.x, roomBounds.min.y + npcHeight / 2, transform.position.z);

            // Set the NPC as a child of the room
            transform.SetParent(room.transform, true);

            // Call AssignRoom method
            npcComponent.AssignRoom(room);

            // Assign this NPC to the room
            room.SetNPC(this);

            // Ensure the NPC's local scale is reset to 1
            //transform.localScale = Vector3.one;

            // Start pacing routine using the NPC component
            npcComponent.StartPacing(roomGameObject); // Correct way to start pacing
        }
        else
        {
            transform.position = startPosition;
            transform.parent = originalParent;
        }
    }



    public void ReturnToStartPosition()
    {
        transform.position = startPosition; // Snap back to the original saved position
    }
}
