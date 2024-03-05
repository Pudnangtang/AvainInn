using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    public WaitingAreaManager waitingAreaManager;

    public GameObject npcPrefab;
    public float spawnInterval = 5f; // Time in seconds between each spawn
    public Transform spawnPoint;

    private float timer;

    void Update()
    {
        // Check if the waiting area is full
        if (waitingAreaManager.waitingNPCs.Count >= waitingAreaManager.maxWaitingNPCs)
        {
            return; // Stop spawning if the waiting area is full
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnNPC();
            timer = 0f;
        }
    }

    void SpawnNPC()
    {
        GameObject newNPC = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        waitingAreaManager.AddToWaitingArea(newNPC);
        // ... Additional code ...
    }
}
