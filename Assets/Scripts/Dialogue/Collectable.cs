using UnityEngine;
using Ink.Runtime;

public class Collectable : MonoBehaviour
{
    public string collectableName; // The name of the collectable item in Ink

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter triggered with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            // Access the DialogueManager's instance
            DialogueManager dialogueManager = DialogueManager.GetInstance();
            if (dialogueManager == null)
            {
                Debug.LogError("DialogueManager instance is null.");
                return;
            }

            // Get the Story instance from DialogueManager
            Story story = dialogueManager.GetInkStory();
            if (story == null)
            {
                Debug.LogError("Ink story is null in DialogueManager.");
                return;
            }

            // Set the variable within the Ink story
            story.variablesState[collectableName] = true;
            Debug.Log(collectableName + " collected: " + story.variablesState[collectableName].ToString());

            // Optionally, communicate back to the DialogueManager that a refresh is needed
            dialogueManager.RefreshDialogueUI();

            // Disable the collectable object to simulate collection
            gameObject.SetActive(false);
        }
    }
}

