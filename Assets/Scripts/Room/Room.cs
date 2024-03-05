using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour
{
    // Time of the last mouse click. Used to detect double-clicks.
    private float lastClickTime = 0f;
    // Time interval to register as a double-click.
    private const float DOUBLE_CLICK_TIME = 0.2f;

    // Reference to the current NPC in the room.
    public DraggableNPC currentNPC;

    // Floor number to identify the room's floor in the building.
    public int floorNumber; // Assign this through the Unity Inspector for each room

    // This method is called whenever the room is clicked.
    void OnMouseDown()
    {
        // Calculate the time since the last click.
        float timeSinceLastClick = Time.time - lastClickTime;

        // If the time since the last click is less than the threshold for a double-click, register it as a double-click.
        if (timeSinceLastClick <= DOUBLE_CLICK_TIME)
        {
            // If a double-click is detected, focus the camera on this room.
            // The CameraDragMovement script should have a method that handles the camera focus.
            CameraDragMovement.Instance.FocusOnRoom(transform.position);
        }

        // Update the lastClickTime to the current time.
        lastClickTime = Time.time;
    }

    // Check if the room is currently occupied by an NPC.
    public bool IsOccupied()
    {
        return currentNPC != null;
    }

    // Set the current NPC reference when an NPC is placed in the room.
    public void SetNPC(DraggableNPC npc)
    {
        currentNPC = npc;
    }

    // Clear the current NPC reference when an NPC leaves the room.
    public void ClearNPC()
    {
        currentNPC = null;
    }

    
    // ... Other room-related methods ...
}
