using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // Required for UI event handling

public class SpeechBubble : MonoBehaviour, IPointerClickHandler
{
    public NPC ownerNPC;

    // This function is called when the object is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ownerNPC != null)
        {
            ownerNPC.OnSpeechBubbleClicked();
        }
        else
        {
            Debug.LogError("No ownerNPC assigned to the speech bubble!");
        }
    }

    /*public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Entering Dialogue Mode");
            ownerNPC.OnSpeechBubbleClicked();
        }
    }*/
}
