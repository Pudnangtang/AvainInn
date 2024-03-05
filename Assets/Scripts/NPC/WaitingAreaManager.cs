using UnityEngine;
using System.Collections.Generic;

public class WaitingAreaManager : MonoBehaviour
{
    public List<GameObject> waitingNPCs = new List<GameObject>();
    public float spaceBetweenNPCs = 1.0f; // The space between each NPC in line
    public Vector3 startPosition; // The starting position of the first NPC in line
    public int maxWaitingNPCs = 4; // Maximum number of NPCs in the waiting area

    public void AddToWaitingArea(GameObject npc)
    {
        if (waitingNPCs.Count >= maxWaitingNPCs)
        {
            // Optional: Handle the case when the waiting area is full
            return;
        }

        // Position the new NPC behind the last NPC in the waiting line
        Vector3 newPosition = startPosition - new Vector3(waitingNPCs.Count * spaceBetweenNPCs, 0, 0);
        npc.transform.position = newPosition;

        waitingNPCs.Add(npc);
    }

    // Call this method when an NPC is placed in a room
    public void RemoveFromWaitingArea(GameObject npc)
    {
        if (waitingNPCs.Contains(npc))
        {
            waitingNPCs.Remove(npc);
            UpdatePositions();
        }

        UpdatePositions(); // Make sure to update the positions of NPCs in line
    }

    // Update the positions of NPCs in the waiting area
    private void UpdatePositions()
    {
        for (int i = 0; i < waitingNPCs.Count; i++)
        {
            Vector3 newPosition = startPosition - new Vector3(i * spaceBetweenNPCs, 0, 0);
            waitingNPCs[i].transform.position = newPosition;
        }
    }

    // ... Other methods ...
}
