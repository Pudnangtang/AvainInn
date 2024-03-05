using UnityEngine;
using System.Collections;
using UnityEngine.UI; // If using Unity UI
using UnityEngine.EventSystems; // If using EventTrigger

public class NPC : MonoBehaviour
{

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string KnotName;

    [Header("NPC Voice Settings")]
    [SerializeField] private float npcTypingSpeed = 0.05f; // Default typing speed for the NPC
    [SerializeField] private float npcVoicePitch = 1.0f;   // Default voice pitch for the NPC
    [SerializeField] private float npcBeepFrequency = 1.0f;   // Default beep frequency for the NPC


    [Range(0, 100)]
    public int happiness = 50; // Default happiness level
    public int preferredFloor; // The floor number that this NPC prefers
    public HappinessDisplay happinessDisplay; // Reference to the HappinessDisplay

    public float minStayTime; // Minimum amount of time to stay in the room
    public float maxStayTime; // Maximum amount of time to stay in the room

    private Coroutine pacingCoroutine;
    private Coroutine leaveRoomCoroutine;

    private float timeEnteredRoom; // Time when the NPC entered the room

    public const float chanceToShowBubble = 0.9f; // 20% chance to show the bubble

    private WaitingAreaManager waitingAreaManager;
    public GameObject speechBubblePrefab; // Assign in the inspector
    private GameObject speechBubbleInstance;
    private DialogueManager dialogueManager; // Assume you have a DialogueManager in the scene

    private bool isDialogueActive = false;


    private void Start()
    {
        // Initialize the happiness display at the start
        UpdateHappinessState();

        // Find the WaitingAreaManager instance in the scene
        waitingAreaManager = FindObjectOfType<WaitingAreaManager>();

        // Ensure there is a WaitingAreaManager in the scene
        if (waitingAreaManager == null)
        {
            Debug.LogError("No WaitingAreaManager found in the scene.");
        }

        // Find the DialogueManager in the scene
        dialogueManager = FindObjectOfType<DialogueManager>();

        // Start the coroutine to attempt to show the speech bubble
        StartCoroutine(TryShowSpeechBubble());
    }

    private void Update()
    {
        // Start the coroutine to attempt to show the speech bubble
        StartCoroutine(TryShowSpeechBubble());
    }

    private IEnumerator TryShowSpeechBubble()
    {
        while (true) // This loop runs indefinitely
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            // Only try to show the speech bubble if dialogue is not active
            // and there isn't already a speech bubble instance
            if (!isDialogueActive && speechBubbleInstance == null)
            {
                // There's a 20% chance to show the bubble each second
                if (Random.value <= 0.2f) // Use 0.2f for 20%
                {
                    // Show the speech bubble
                    speechBubbleInstance = Instantiate(speechBubblePrefab, transform.position, Quaternion.identity, transform);
                    speechBubbleInstance.transform.localPosition = new Vector3(0, 1.5f, 0); // Adjust the Y value as needed
                    SpeechBubble speechBubbleScript = speechBubbleInstance.GetComponent<SpeechBubble>();
                    if (speechBubbleScript != null)
                    {
                        speechBubbleScript.ownerNPC = this;
                    }
                }
            }
        }
    }


    public void OnSpeechBubbleClicked()
    {
        if (!isDialogueActive)
        {
            // Dialogue gets activated
            isDialogueActive = true;
            if (speechBubbleInstance != null)
            {
                Destroy(speechBubbleInstance); // Destroy the speech bubble when clicked
            }

            // Assuming you've assigned the DialogueManager.Instance somewhere, like in its Awake
            DialogueManager.Instance.StartDialogue(KnotName);

            Debug.Log("Speech bubble clicked");
            // Start the dialogue here, e.g., call the DialogueManager
            
        }
    }

    public IEnumerator WaitAndLeaveRoom(Room room)
    {
        // Wait for a random amount of time within the range before leaving
        float waitTime = Random.Range(minStayTime, maxStayTime);
        yield return new WaitForSeconds(waitTime);

        LeaveRoom(room);
    }

    private void LeaveRoom(Room room)
    {
        // Calculate the total duration of the NPC's stay
        float stayDuration = Time.time - timeEnteredRoom;

        // Calculate the money given based on happiness and stay duration
        int moneyGiven = CalculateMoneyGiven(happiness, stayDuration);

        // Find the Money instance and update the score
        Money money = FindObjectOfType<Money>();
        if (money != null)
        {
            money.AddScore(moneyGiven);
        }
        else
        {
            Debug.LogError("No MoneyManager found in the scene.");
        }

        // Destroy the NPC GameObject
        Destroy(gameObject);
    }

    private int CalculateMoneyGiven(int happiness, float stayDuration)
    {
        // Define the logic for calculating money based on happiness and stay duration
        // For example, base money given for happiness plus a bonus for each second stayed
        int baseMoney = happiness * 1; // Base money given per happiness point
        int bonusMoney = Mathf.FloorToInt(stayDuration); // Additional money for each second stayed

        return baseMoney + bonusMoney;
    }

    // This method would be called by the DialogueManager to end the dialogue
    public void EndDialogue()
    {
        isDialogueActive = false;

        // Show the speech bubble again if needed
        if (speechBubbleInstance != null)
        {
            speechBubbleInstance.SetActive(true);
        }

        // Add any cleanup logic here
    }

    private void OnDestroy()
    {
        // When the NPC is destroyed, make sure to stop the coroutine
        StopAllCoroutines();
    }

    public void AssignRoom(Room room)
    {
        // Record the time when the NPC was assigned to the room
        timeEnteredRoom = Time.time;

        // Check if the NPC is happy with the assigned floor
        if (room.floorNumber == preferredFloor)
        {
            IncreaseHappiness(25); // Increase happiness if on the preferred floor
            Debug.Log("Happiness Increased");
        }
        else
        {
            DecreaseHappiness(15); // Decrease happiness if not on the preferred floor
            Debug.Log("Happiness Decreased");
        }

        if (leaveRoomCoroutine != null)
        {
            StopCoroutine(leaveRoomCoroutine);
        }
        leaveRoomCoroutine = StartCoroutine(WaitAndLeaveRoom(room));
        // Move the NPC to the floor of the room
        //DropToFloor(room);

        // Update NPC's state based on its happiness
        UpdateHappinessState();

        // Start pacing behavior
        StartPacing(room.gameObject);

        // Remove the NPC from the waiting area
        if (waitingAreaManager != null)
        {
            waitingAreaManager.RemoveFromWaitingArea(gameObject);
        }
    }

    
    
    public void StartPacing(GameObject room)
    {
        if (pacingCoroutine != null)
        {
            StopCoroutine(pacingCoroutine);
        }
        pacingCoroutine = StartCoroutine(PaceInRoom(room));
    }

    public IEnumerator PaceInRoom(GameObject room)
    {
        Bounds roomBounds = room.GetComponent<Collider2D>().bounds;
        float leftLimit = roomBounds.min.x;
        float rightLimit = roomBounds.max.x;

        float paceSpeed = 2f; // Speed of NPC movement
        float targetX = rightLimit; // Initial target

        while (true) // Infinite loop for continuous pacing
        {
            // Move towards the target
            Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, paceSpeed * Time.deltaTime);

            // Check if reached target
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                // Switch target
                targetX = (targetX == rightLimit) ? leftLimit : rightLimit;
            }

            yield return null; // Wait for the next frame
        }
    }


        public void StopPacing()
    {
        if (pacingCoroutine != null)
        {
            StopCoroutine(pacingCoroutine);
            pacingCoroutine = null;
        }
    }

    public void IncreaseHappiness(int amount)
    {
        happiness = Mathf.Min(happiness + amount, 100);
        UpdateHappinessState();
    }

    public void DecreaseHappiness(int amount)
    {
        happiness = Mathf.Max(happiness - amount, 0);
        UpdateHappinessState();
    }

    private void UpdateHappinessState()
    {
        // Update the NPC's state based on the current happiness value
        if (happinessDisplay != null)
        {
            happinessDisplay.UpdateHappiness(happiness);
        }
    }
}
