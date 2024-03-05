using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Method to place an NPC into a room, called when an NPC is dropped into a room
    public void PlaceNPCInRoom(DraggableNPC draggableNPC, Room room)
    {
        if (!room.IsOccupied())
        {
            // Set the NPC to the floor of the room
            NPC npc = draggableNPC.GetComponent<NPC>();
            if (npc != null)
            {
                room.SetNPC(draggableNPC); // Set the NPC reference in the room
                npc.AssignRoom(room); // Assign the room to the NPC which handles happiness changes
                npc.StartPacing(room.gameObject); // Start pacing behavior
            }
        }
        else
        {
            Debug.Log("Room is already occupied.");
            // Handle the case when the room is occupied, e.g., snap the NPC back to its original position
            draggableNPC.ReturnToStartPosition();
        }
    }
}
